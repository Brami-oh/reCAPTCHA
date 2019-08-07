using System;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Finoaker.Web.Core;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// reCAPTCHA properties to be used in by client side script. Instance is serlialized to Json.
    /// </summary>
    [DataContract]
    internal class RecaptchaProps
    {
        private const string RecaptchaApiScriptUrl = "https://www.google.com/recaptcha/api.js";
        private const string EmbeddedScriptFileName = "Scripts/dist/recaptcha.min.js";
        private const string ScriptPropsVarName = "f_recaptcha__props";
        private const string ContainerCssClassName = "f-recaptcha-container";
        private const string RecaptchaCssClassName = "g-recaptcha";

        public RecaptchaProps(
            RecaptchaType type,
            string siteKey,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null,
            ThemeType? theme = null,
            int? tabIndex = null,
            SizeType? size = null,
            BadgeType? badge = null,
            string action = null)
        {
            RenderProps = new RenderProps(
                type,
                siteKey,
                theme: theme,
                tabIndex: tabIndex,
                size: size,
                badge: badge,
                action: action);

            Type = type;
            Callback = callback.TrimToNull();
            ExpiredCallback = expiredCallback.TrimToNull();
            ErrorCallback = errorCallback.TrimToNull();
        }

        [DataMember(Name = "type")]
        public RecaptchaType Type { get; private set; }

        [DataMember(Name = "hiddenInputId")]
        public string HiddenInputId { get; private set; }

        [DataMember(Name = "scriptId")]
        public readonly string ScriptId = "f-recaptcha--script";

        [DataMember(Name = "onLoadCallback")]
        public readonly string OnLoadCallback = "f_recaptcha__onload";

        [DataMember(Name = "callback")]
        public string Callback { get; private set; }

        [DataMember(Name = "expiredCallback")]
        public string ExpiredCallback { get; private set; }

        [DataMember(Name = "errorCallback")]
        public string ErrorCallback { get; private set; }

        [DataMember(Name = "renderProps")]
        public RenderProps RenderProps { get; private set; }

        /// <summary>
        /// Generates Html required to render reCAPTCHA using instance properties.
        /// </summary>
        /// <param name="input">Hidden &lt;input&gt; element that will hold the reCAPTCHA challenge response token.</param>
        /// <returns></returns>
        public TagBuilder GenerateHtml(TagBuilder input)
        {
            // container for reCAPTCHA elements
            var tagBuilder = new TagBuilder("div");

            tagBuilder.AddCssClass(ContainerCssClassName);

            if (!string.IsNullOrWhiteSpace(input?.Attributes["id"]) || Type == RecaptchaType.V3)
            {
                HiddenInputId = input?.Attributes["id"]?.Trim();

                tagBuilder.InnerHtml
                    .AppendHtml(input)
                    .AppendHtml(GenerateCustomScriptTag())
                    .AppendHtml(GenerateScriptTag());
            }
            else if (input is null && Type == RecaptchaType.V2Checkbox)
            {
                tagBuilder.InnerHtml
                    .AppendHtml(GenerateDivTag())
                    .AppendHtml(GenerateV2ScriptTag());
            }
            else
            {
                // used to catch any unexpected types or use cases.
                throw new ArgumentException("reCAPTCHA type not supported.", nameof(Type));
            }
            return tagBuilder;
        }

        private TagBuilder GenerateV2ScriptTag()
        {
            var tagBuilder = new TagBuilder("script");

            tagBuilder.Attributes.Add("src", RecaptchaApiScriptUrl);
            tagBuilder.Attributes.Add("defer", null);
            tagBuilder.Attributes.Add("async", null);

            return tagBuilder;
        }

        private TagBuilder GenerateScriptTag()
        {
            var tagBuilder = new TagBuilder("script");

            tagBuilder.Attributes.Add("src", $"{RecaptchaApiScriptUrl}?onload={OnLoadCallback}&render=explicit");

            return tagBuilder;
        }

        private void GenerateV3Attributes(IDictionary<string, string> attributes)
        {
            attributes.Add("data-size", SizeType.Invisible.ToString().ToLower());

            if (RenderProps.Badge != null)
            {
                attributes.Add("data-badge", RenderProps.Badge.ToString());
            }
        }

        private void GenerateV2CheckboxAttributes(IDictionary<string, string> attributes)
        {
            if (!string.IsNullOrWhiteSpace(ExpiredCallback))
            {
                attributes.Add("data-expired-callback", ExpiredCallback);
            }

            if (!string.IsNullOrWhiteSpace(ErrorCallback))
            {
                attributes.Add("data-error-callback", ErrorCallback);
            }

            if (RenderProps.Size != null)
            {
                attributes.Add("data-size", RenderProps.Size.ToString());
            }

            if (RenderProps.Theme != null)
            {
                attributes.Add("data-theme", RenderProps.Theme.ToString());
            }

            if (RenderProps.TabIndex.HasValue)
            {
                attributes.Add("data-tabindex", RenderProps.TabIndex.ToString());
            }
        }

        private IDictionary<string, string> GenerateAttributes()
        {
            var attributes = new Dictionary<string, string>();

            attributes.Add("class", RecaptchaCssClassName);

            attributes.Add("data-sitekey", RenderProps.SiteKey);

            if (!string.IsNullOrWhiteSpace(Callback))
            {
                attributes.Add("data-callback", Callback);
            }

            if (Type == RecaptchaType.V2Checkbox)
            {
                GenerateV2CheckboxAttributes(attributes);
            }
            else if (Type == RecaptchaType.V3)
            {
                GenerateV3Attributes(attributes);
            }

            return attributes;
        }

        private TagBuilder GenerateDivTag()
        {
            // User doesn't want to store the challenge result and so use the basic method for rendering 
            // reCAPTCHA that has properties added to a <div> element as "data-*" attributes.
            var tag = new TagBuilder("div");

            tag.MergeAttributes(GenerateAttributes());

            return tag;
        }

        private TagBuilder GenerateCustomScriptTag()
        {
            // get the script file content from embedded resources
            var script = GenerateScriptContent();

            var scriptTag = new TagBuilder("script");

            scriptTag.Attributes.Add("id", ScriptId);
            scriptTag.InnerHtml.AppendHtml(script);

            return scriptTag;
        }

        private string GenerateScriptContent()
        {
            // get the script file content from embedded resources
            var script = GetEmbeddedResource(
                    EmbeddedScriptFileName,
                    typeof(RecaptchaProps).GetTypeInfo().Assembly);

            if (script is null)
            {
                throw new FileNotFoundException("Embedded Javascript file not found.", EmbeddedScriptFileName);
            }

            // convert the RecaptchaProps object to a JSON object
            var stream = new MemoryStream();

            new DataContractJsonSerializer(
                typeof(RecaptchaProps), 
                new DataContractJsonSerializerSettings()
                {
                    SerializeReadOnlyTypes = true
                })
                .WriteObject(stream, this);

            stream.Position = 0;

            // inject the Json RecaptchaProps object into the script.
            return $"var {ScriptPropsVarName}={new StreamReader(stream).ReadToEnd()};{script}";
        }

        private static string GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            resourceName = $"{assembly.GetName().Name}.{resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".")}";

            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream is null)
                {
                    return null;
                }

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
