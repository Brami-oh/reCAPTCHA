/// <binding BeforeBuild='default' Clean='clean' />
module.exports = function (grunt) {
    grunt.initConfig({
        ts: {
            default: {
                tsconfig: "Scripts/tsconfig.json"
            }
        },
        uglify: {
            scripts: {
                files: {
                    "Scripts/dist/recaptcha.min.js": [
                        "Scripts/RecaptchaType.js",
                        "Scripts/Recaptcha.js"
                    ],
                }
            }
        },
        clean: {
            js: ["Scripts/*.js"]
        }
    });

    grunt.loadNpmTasks("grunt-ts");
    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-contrib-clean");

    grunt.registerTask("default", ["ts", "uglify", "clean"]);
};