/// <binding BeforeBuild='default' Clean='clean' />
module.exports = function (grunt) {
    grunt.initConfig({
        ts: {
            scripts: {
                src: ["scripts/*.ts"],
                options: {
                    comments: false,
                    sourceMap: false,
                    target: "es5"
                }
            }
        },
        jshint: {
            scripts: ["scripts/*.js"]
        },
        uglify: {
            scripts: {
                files: {
                    "scripts/dist/RecaptchaV2Checkbox.min.js": ["scripts/RecaptchaV2Checkbox.js"],
                    "scripts/dist/RecaptchaV3.min.js": ["scripts/RecaptchaV3.js"]
                }
            }
        },
        clean: {
            js: ["scripts/*.js"]
        }
    });

    grunt.loadNpmTasks("grunt-ts");
    grunt.loadNpmTasks("grunt-contrib-jshint");
    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-contrib-clean");

    grunt.registerTask("default", ["ts", "jshint", "uglify", "clean"]);
};