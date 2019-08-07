// Placeholder for reCAPTCHA properties object, which is injected server side.
// NOTE: If you change the name here, you must also change server side.
declare const f_recaptcha__props: IRecaptchaProps;

(function () {

    const props = f_recaptcha__props;

    if (!props) {
        throw Error(`Props cannot be null or undefined`);
    }
    // Function to <div> element to host the reCAPTCHA
    const createDiv = (): HTMLDivElement => {

        switch (props.type) {
            case RecaptchaType.v2checkbox:
                return <HTMLDivElement>document
                    .getElementById(props.scriptId)
                    .insertAdjacentElement(`afterend`, document.createElement(`div`));
            case RecaptchaType.v3:
                return document
                    .body
                    .appendChild(document.createElement(`div`));
            default:
                throw Error(`reCAPTCHA type not supported.`);
        }
    };
    // Function to be called when reCAPTCHA assets have loaded.
    if (!window[props.onLoadCallback]) {

        window[props.onLoadCallback] = () => {

            const recaptcha = <IRecaptcha>window[`grecaptcha`];

            // render the reCAPTCHA component
            const clientId = recaptcha.render(div, props.renderProps);

            // automatically execute for v3 only
            if (props.type == RecaptchaType.v3) {
                recaptcha.execute(clientId, {
                    action: props.renderProps.action
                });
            }
        };
    }
    // Function to be called on successful challenge response
    if (!window[props.renderProps.callback]) {

        window[props.renderProps.callback] = (token: string) => {

            if (token && input) {
                // assign challenge response token to <input> element
                input.value = token;
            }

            if (window[props.callback]) {
                // execute user callback if supplied.
                window[props.callback](token);
            }
        };
    }
    // Function to be called when an error occurs.
    if (!window[props.renderProps[`error-callback`]]) {

        window[props.renderProps[`error-callback`]] = () => {

            if (input) {
                // reset <input> element
                input.value = null;
            }

            if (window[props.errorCallback]) {
                // execute user callback if supplied.
                window[props.errorCallback]();
            }
        };
    }
    // Function to be called when challenge response expires.
    if (!window[props.renderProps[`expired-callback`]]) {

        window[props.renderProps[`expired-callback`]] = () => {

            if (input) {
                // reset <input> element
                input.value = null;
            }

            if (window[props.expiredCallback]) {
                // execute user callback if supplied.
                window[props.expiredCallback]();
            }
        };
    }
    // get ref to hidden <input> element.
    const input = <HTMLInputElement>document.getElementById(props.hiddenInputId);
    // create the <div> element
    const div = createDiv();
})();