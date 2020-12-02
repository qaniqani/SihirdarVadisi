using System.Web.Optimization;

namespace AdminProject
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(AdminScriptBundle());
            bundles.Add(AdminStyleBundle());
            bundles.Add(WebScriptBundle());
            bundles.Add(WebStyleBundle());

            BundleTable.EnableOptimizations = false;
        }

        private static Bundle AdminScriptBundle()
        {
            var bundle = new ScriptBundle("~/main-effect").Include(
                        "~/assets/js/app.js",
                        "~/assets/js/preloader.js",
                        "~/assets/js/bootstrap.js",
                        "~/assets/js/load.js",
                        "~/assets/js/main.js",
                        "~/assets/js/fancybox/jquery.fancybox.pack.js",
                        "~/assets/js/fancybox/jquery.fancybox.js",
                        "~/assets/js/iCheck/jquery.icheck.js",
                        "~/assets/js/switch/bootstrap-switch.js",
                        "~/assets/js/footable/js/footable.js",
                        "~/assets/js/footable/js/footable.sort.js",
                        "~/assets/js/footable/js/footable.filter.js",
                        "~/assets/js/footable/js/footable.paginate.js",
                        "~/assets/js/footable/js/footable.paginate.js",
                        "~/assets/js/jquery.mjs.nestedSortable.js",
                        "~/assets/tinymce/tinymce.min.js",
                        "~/assets/js/jquery.validationEngine.js",
                        "~/assets/js/jquery.validationEngine-tr.js",
                        "~/assets/js/wizard/build/jquery.steps.js",
                        "~/assets/js/wizard/jquery.stepy.js",
                        "~/assets/js/tree/jquery.treeview.js",
                        "~/assets/js/pnotify/jquery.pnotify.min.js",
                        "~/assets/js/datepicker/bootstrap-datetimepicker.js",
                        "~/assets/js/datetimepicker/jquery.datetimepicker.full.min.js",
                        "~/assets/js/datepicker/bootstrap-datepicker.js",
                        "~/assets/js/multi.file.js",
                        "~/assets/js/scripts.js"
                      );

            return bundle;
        }

        private static Bundle AdminStyleBundle()
        {
            var bundle = new StyleBundle("~/css").Include(
                        "~/assets/css/style.css",
                        "~/assets/css/loader-style.css",
                        "~/assets/css/bootstrap.css",
                        "~/assets/js/iCheck/flat/all.css",
                        "~/assets/js/iCheck/line/all.css",
                        "~/assets/js/colorPicker/bootstrap-colorpicker.css",
                        "~/assets/js/switch/bootstrap-switch.css",
                        "~/assets/js/validate/validate.css",
                        "~/assets/js/idealform/css/jquery.idealforms.css",
                        "~/assets/js/footable/css/footable.core.css",
                        "~/assets/js/footable/css/footable.standalone.css",
                        "~/assets/js/footable/css/footable-demos.css",
                        "~/assets/js/dataTable/lib/jquery.dataTables/css/DT_bootstrap.css",
                        "~/assets/js/dataTable/css/datatables.responsive.css",
                        "~/assets/js/wizard/css/jquery.steps.css",
                        "~/assets/js/wizard/jquery.stepy.css",
                        "~/assets/js/tabs/acc-wizard.min.css",
                        "~/assets/js/tree/jquery.treeview.css",
                        "~/assets/js/tree/treetable/stylesheets/jquery.treetable.css",
                        "~/assets/js/tree/treetable/stylesheets/jquery.treetable.theme.default.css",
                        "~/assets/css/validationEngine.jquery.css",
                        "~/assets/js/fancybox/jquery.fancybox.css",
                        "~/assets/js/datetimepicker/jquery.datetimepicker.min.css",
                        "~/assets/js/datepicker/datepicker.css"
                      //"~/assets/",
                      //"~/assets/",
                      );

            return bundle;
        }

        private static Bundle WebScriptBundle()
        {
            var bundle = new ScriptBundle("~/web-scripts").Include(
                        "~/html/scripts/libs/jquery-1.12.4.min.js",
                        "~/html/scripts/libs/jquery.pireloader.min.js",
                        "~/html/scripts/libs/bootstrap.min.js",
                        "~/html/scripts/libs/material.min.js",
                        "~/html/scripts/libs/ripples.min.js",
                        "~/html/scripts/libs/jetmenu.min.js",
                        "~/html/scripts/libs/jquery.dotdotdot.min.js",
                        "~/html/scripts/libs/responsiveslides.min.js",
                        "~/html/scripts/libs/timetable.min.js",
                        "~/html/scripts/libs/jquery.magnific-popup.min.js",
                        "~/html/scripts/libs/jquery.validationEngine-tr.js",
                        "~/html/scripts/libs/jquery.validationEngine.min.js",
                        "~/html/scripts/libs/fuckadblock.min.js",
                        "~/html/scripts/libs/fileinput.js",
                        "~/html/scripts/application.min.js",
                        "~/html/scripts/forms.min.js"
                      );

            return bundle;
        }

        private static Bundle WebStyleBundle()
        {
            var bundle = new ScriptBundle("~/web-styles").Include(
                        "~/html/assets/libs/jquery.pireloader.min.css",
                        "~/html/assets/libs/bootstrap.min.css",
                        "~/html/assets/libs/bootstrap-material-design.min.css",
                        "~/html/assets/libs/ripples.min.css",
                        "~/html/assets/libs/responsiveslides.min.css",
                        "~/html/assets/libs/jetmenu.min.css",
                        "~/html/assets/libs/timetable.min.css",
                        "~/html/assets/libs/magnific-popup.min.css",
                        "~/html/assets/libs/validationEngine.jquery.min.css",
                        "~/html/assets/libs/fileinput.js",
                        "~/html/assets/global.min.css"
                      );

            return bundle;
        }
    }
}