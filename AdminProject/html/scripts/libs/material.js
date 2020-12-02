!function (a) { function b(a) { return "undefined" == typeof a.which ? !0 : "number" == typeof a.which && a.which > 0 ? !a.ctrlKey && !a.metaKey && !a.altKey && 8 != a.which && 9 != a.which && 13 != a.which && 16 != a.which && 17 != a.which && 20 != a.which && 27 != a.which : !1 } function c(b) { var c = a(b); c.prop("disabled") || c.closest(".form-group").addClass("is-focused") } function d(a, b) { var c; return c = a.hasClass("checkbox-inline") || a.hasClass("radio-inline") ? a : a.closest(".checkbox").length ? a.closest(".checkbox") : a.closest(".radio"), c.toggleClass("disabled", b) } function e(b) { var e = !1; (b.is(a.material.options.checkboxElements) || b.is(a.material.options.radioElements)) && (e = !0), b.closest("label").hover(function () { var b = a(this).find("input"), f = b.prop("disabled"); e && d(a(this), f), f || c(b) }, function () { f(a(this).find("input")) }) } function f(b) { a(b).closest(".form-group").removeClass("is-focused") } a.expr[":"].notmdproc = function (b) { return a(b).data("mdproc") ? !1 : !0 }, a.material = { options: { validate: !0, input: !0, ripples: !0, checkbox: !0, togglebutton: !0, radio: !0, arrive: !0, autofill: !1, withRipples: [".btn:not(.btn-link)", ".card-image", ".navbar a:not(.withoutripple)", ".dropdown-menu a", ".nav-tabs a:not(.withoutripple)", ".withripple", ".pagination li:not(.active):not(.disabled) a:not(.withoutripple)"].join(","), inputElements: "input.form-control, textarea.form-control, select.form-control", checkboxElements: ".checkbox > label > input[type=checkbox], label.checkbox-inline > input[type=checkbox]", togglebuttonElements: ".togglebutton > label > input[type=checkbox]", radioElements: ".radio > label > input[type=radio], label.radio-inline > input[type=radio]" }, checkbox: function (b) { var c = a(b ? b : this.options.checkboxElements).filter(":notmdproc").data("mdproc", !0).after("<span class='checkbox-material'><span class='check'></span></span>"); e(c) }, togglebutton: function (b) { var c = a(b ? b : this.options.togglebuttonElements).filter(":notmdproc").data("mdproc", !0).after("<span class='toggle'></span>"); e(c) }, radio: function (b) { var c = a(b ? b : this.options.radioElements).filter(":notmdproc").data("mdproc", !0).after("<span class='circle'></span><span class='check'></span>"); e(c) }, input: function (b) { a(b ? b : this.options.inputElements).filter(":notmdproc").data("mdproc", !0).each(function () { var b = a(this), c = b.closest(".form-group"); 0 !== c.length || "hidden" === b.attr("type") || b.attr("hidden") || (b.wrap("<div class='form-group'></div>"), c = b.closest(".form-group")), b.attr("data-hint") && (b.after("<p class='help-block'>" + b.attr("data-hint") + "</p>"), b.removeAttr("data-hint")); var d = { "input-lg": "form-group-lg", "input-sm": "form-group-sm" }; if (a.each(d, function (a, d) { b.hasClass(a) && (b.removeClass(a), c.addClass(d)) }), b.hasClass("floating-label")) { var e = b.attr("placeholder"); b.attr("placeholder", null).removeClass("floating-label"); var f = b.attr("id"), g = ""; f && (g = "for='" + f + "'"), c.addClass("label-floating"), b.after("<label " + g + "class='control-label'>" + e + "</label>") } (null === b.val() || "undefined" == b.val() || "" === b.val()) && c.addClass("is-empty"), c.find("input[type=file]").length > 0 && c.addClass("is-fileinput") }) }, attachInputEventHandlers: function () { var d = this.options.validate; a(document).on("keydown paste", ".form-control", function (c) { b(c) && a(this).closest(".form-group").removeClass("is-empty") }).on("keyup change", ".form-control", function () { var b = a(this), c = b.closest(".form-group"), e = "undefined" == typeof b[0].checkValidity || b[0].checkValidity(); "" === b.val() ? c.addClass("is-empty") : c.removeClass("is-empty"), d && (e ? c.removeClass("has-error") : c.addClass("has-error")) }).on("focus", ".form-control, .form-group.is-fileinput", function () { c(this) }).on("blur", ".form-control, .form-group.is-fileinput", function () { f(this) }).on("change", ".form-group input", function () { var b = a(this); if ("file" != b.attr("type")) { var c = b.closest(".form-group"), d = b.val(); d ? c.removeClass("is-empty") : c.addClass("is-empty") } }).on("change", ".form-group.is-fileinput input[type='file']", function () { var b = a(this), c = b.closest(".form-group"), d = ""; a.each(this.files, function (a, b) { d += b.name + ", " }), d = d.substring(0, d.length - 2), d ? c.removeClass("is-empty") : c.addClass("is-empty"), c.find("input.form-control[readonly]").val(d) }) }, ripples: function (b) { a(b ? b : this.options.withRipples).ripples() }, autofill: function () { var b = setInterval(function () { a("input[type!=checkbox]").each(function () { var b = a(this); b.val() && b.val() !== b.attr("value") && b.trigger("change") }) }, 100); setTimeout(function () { clearInterval(b) }, 1e4) }, attachAutofillEventHandlers: function () { var b; a(document).on("focus", "input", function () { var c = a(this).parents("form").find("input").not("[type=file]"); b = setInterval(function () { c.each(function () { var b = a(this); b.val() !== b.attr("value") && b.trigger("change") }) }, 100) }).on("blur", ".form-group input", function () { clearInterval(b) }) }, init: function (b) { this.options = a.extend({}, this.options, b); var c = a(document); a.fn.ripples && this.options.ripples && this.ripples(), this.options.input && (this.input(), this.attachInputEventHandlers()), this.options.checkbox && this.checkbox(), this.options.togglebutton && this.togglebutton(), this.options.radio && this.radio(), this.options.autofill && (this.autofill(), this.attachAutofillEventHandlers()), document.arrive && this.options.arrive && (a.fn.ripples && this.options.ripples && c.arrive(this.options.withRipples, function () { a.material.ripples(a(this)) }), this.options.input && c.arrive(this.options.inputElements, function () { a.material.input(a(this)) }), this.options.checkbox && c.arrive(this.options.checkboxElements, function () { a.material.checkbox(a(this)) }), this.options.radio && c.arrive(this.options.radioElements, function () { a.material.radio(a(this)) }), this.options.togglebutton && c.arrive(this.options.togglebuttonElements, function () { a.material.togglebutton(a(this)) })) } } }(jQuery);/* globals jQuery, window, document */

(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // Node/CommonJS
        module.exports = factory(require('jquery'));
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {

    var methods = {
        options: {
            "optionClass": "",
            "dropdownClass": "",
            "autoinit": false,
            "callback": false,
            "onSelected": false,
            "destroy": function (element) {
                this.destroy(element);
            },
            "dynamicOptLabel": "Add a new option..."
        },
        init: function (options) {

            // Apply user options if user has defined some
            if (options) {
                options = $.extend(methods.options, options);
            } else {
                options = methods.options;
            }

            function initElement($select) {
                // Don't do anything if this is not a select or if this select was already initialized
                if ($select.data("dropdownjs") || !$select.is("select")) {
                    return;
                }

                // Is it a multi select?
                var multi = $select.attr("multiple");

                // Does it allow to create new options dynamically?
                var dynamicOptions = $select.attr("data-dynamic-opts"),
                    $dynamicInput = $();

                // Create the dropdown wrapper
                var $dropdown = $("<div></div>");
                $dropdown.addClass("dropdownjs").addClass(options.dropdownClass);
                $dropdown.data("select", $select);

                // Create the fake input used as "select" element and cache it as $input
                var $input = $("<input type=text readonly class=fakeinput>");
                if ($.material) { $input.data("mdproc", true); }
                // Append it to the dropdown wrapper
                $dropdown.append($input);

                // Create the UL that will be used as dropdown and cache it AS $ul
                var $ul = $("<ul></ul>");
                $ul.data("select", $select);

                // Append it to the dropdown
                $dropdown.append($ul);

                // Transfer the placeholder attribute
                $input.attr("placeholder", $select.attr("placeholder"));

                // Loop trough options and transfer them to the dropdown menu
                $select.find("option").each(function () {
                    // Cache $(this)
                    var $this = $(this);
                    methods._addOption($ul, $this);

                });

                // If this select allows dynamic options add the widget
                if (dynamicOptions) {
                    $dynamicInput = $("<li class=dropdownjs-add></li>");
                    $dynamicInput.append("<input>");
                    $dynamicInput.find("input").attr("placeholder", options.dynamicOptLabel);
                    $ul.append($dynamicInput);
                }



                // Cache the dropdown options
                var selectOptions = $dropdown.find("li");

                // If is a single select, selected the first one or the last with selected attribute
                if (!multi) {
                    var $selected;
                    if ($select.find(":selected").length) {
                        $selected = $select.find(":selected").last();
                    }
                    else {
                        $selected = $select.find("option, li").first();
                        // $selected = $select.find("option").first();
                    }
                    methods._select($dropdown, $selected);
                } else {
                    var selectors = [], val = $select.val()
                    for (var i in val) {
                        selectors.push('li[value=' + val[i] + ']')
                    }
                    if (selectors.length > 0) {
                        var $target = $dropdown.find(selectors.join(','));
                        $target.removeClass("selected");
                        methods._select($dropdown, $target);
                    }
                }

                // Transfer the classes of the select to the input dropdown
                $input.addClass($select[0].className);

                // Hide the old and ugly select
                $select.hide().attr("data-dropdownjs", true);

                // Bring to life our awesome dropdownjs
                $select.after($dropdown);

                // Call the callback
                if (options.callback) {
                    options.callback($dropdown);
                }

                //---------------------------------------//
                // DROPDOWN EVENTS                       //
                //---------------------------------------//

                // On click, set the clicked one as selected
                $ul.on("click", "li:not(.dropdownjs-add)", function (e) {
                    methods._select($dropdown, $(this));
                    // trigger change event, if declared on the original selector
                    $select.change();
                });
                $ul.on("keydown", "li:not(.dropdownjs-add)", function (e) {
                    if (e.which === 27) {
                        $(".dropdownjs > ul > li").attr("tabindex", -1);
                        return $input.removeClass("focus").blur();
                    }
                    if (e.which === 32 && !$(e.target).is("input")) {
                        methods._select($dropdown, $(this));
                        return false;
                    }
                });

                $ul.on("focus", "li:not(.dropdownjs-add)", function () {
                    if ($select.is(":disabled")) {
                        return;
                    }
                    $input.addClass("focus");
                });

                // Add new options when the widget is used
                if (dynamicOptions && dynamicOptions.length) {
                    $dynamicInput.on("keydown", function (e) {
                        if (e.which !== 13) return;
                        var $option = $("<option>"),
                            val = $dynamicInput.find("input").val();
                        $dynamicInput.find("input").val("");

                        $option.attr("value", val);
                        $option.text(val);
                        $select.append($option);

                    });
                }

                // Listen for new added options and update dropdown if needed
                $select.on("DOMNodeInserted", function (e) {
                    var $this = $(e.target);
                    if (!$this.val().length) return;

                    methods._addOption($ul, $this);
                    $ul.find("li").not(".dropdownjs-add").attr("tabindex", 0);

                });

                $select.on("DOMNodeRemoved", function (e) {
                    var deletedValue = e.target.getAttribute('value');
                    $ul.find("li[value='" + deletedValue + "']").remove();
                    var $selected;

                    setTimeout(function () {
                        if ($select.find(":selected").length) {
                            $selected = $select.find(":selected").last();
                        }
                        else {
                            $selected = $select.find("option, li").first();
                        }
                        methods._select($dropdown, $selected);
                    }, 100);

                });

                // Update dropdown when using val, need to use .val("value").trigger("change");
                $select.on("change", function (e) {
                    var $this = $(e.target);

                    if (!multi) {
                        var $selected;
                        if ($select.find(":selected").length) {
                            $selected = $select.find(":selected").last();
                        }
                        else {
                            $selected = $select.find("option, li").first();
                        }
                        methods._select($dropdown, $selected);
                    } else {
                        // methods._select($dropdown, $select.find(":selected"));
                    }
                });

                // Used to make the dropdown menu more dropdown-ish
                $input.on("click focus", function (e) {
                    e.stopPropagation();
                    if ($select.is(":disabled")) {
                        return;
                    }
                    $(".dropdownjs > ul > li").attr("tabindex", -1);
                    $(".dropdownjs > input").not($(this)).removeClass("focus").blur();

                    $(".dropdownjs > ul > li").not(".dropdownjs-add").attr("tabindex", 0);

                    // Set height of the dropdown
                    var coords = {
                        top: $(this).offset().top - $(document).scrollTop(),
                        left: $(this).offset().left - $(document).scrollLeft(),
                        bottom: $(window).height() - ($(this).offset().top - $(document).scrollTop()),
                        right: $(window).width() - ($(this).offset().left - $(document).scrollLeft())
                    };

                    var height = coords.bottom;

                    // Decide if place the dropdown below or above the input
                    if (height < 200 && coords.top > coords.bottom) {
                        height = coords.top;
                        $ul.attr("placement", "top-left");
                    } else {
                        $ul.attr("placement", "bottom-left");
                    }

                    $(this).next("ul").css("max-height", height - 20);
                    $(this).addClass("focus");
                });
                // Close every dropdown on click outside
                $(document).on("click", function (e) {

                    // Don't close the multi dropdown if user is clicking inside it
                    if (multi && $(e.target).parents(".dropdownjs").length) return;

                    // Don't close the dropdown if user is clicking inside the dynamic-opts widget
                    if ($(e.target).parents(".dropdownjs-add").length || $(e.target).is(".dropdownjs-add")) return;

                    // Close opened dropdowns
                    $(".dropdownjs > ul > li").attr("tabindex", -1);
                    $input.removeClass("focus");
                });
            }

            if (options.autoinit) {
                $(document).on("DOMNodeInserted", function (e) {
                    var $this = $(e.target);
                    if (!$this.is("select")) {
                        $this = $this.find('select');
                    }
                    if ($this.is(options.autoinit)) {
                        $this.each(function () {
                            initElement($(this));
                        });
                    }
                });
            }

            // Loop trough elements
            $(this).each(function () {
                initElement($(this));
            });
        },
        select: function (target) {
            var $target = $(this).find("[value=\"" + target + "\"]");
            methods._select($(this), $target);
        },
        _select: function ($dropdown, $target) {
            if ($target.is(".dropdownjs-add")) return;

            // Get dropdown's elements
            var $select = $dropdown.data("select"),
                $input = $dropdown.find("input.fakeinput");
            // Is it a multi select?
            var multi = $select.attr("multiple");

            // Cache the dropdown options
            var selectOptions = $dropdown.find("li");

            // Behavior for multiple select
            if (multi) {
                // Toggle option state
                $target.toggleClass("selected");
                // Toggle selection of the clicked option in native select
                $target.each(function () {
                    var $selected = $select.find("[value=\"" + $(this).attr("value") + "\"]");
                    $selected.prop("selected", $(this).hasClass("selected"));
                });
                // Add or remove the value from the input
                var text = [];
                selectOptions.each(function () {
                    if ($(this).hasClass("selected")) {
                        text.push($(this).text());
                    }
                });
                $input.val(text.join(", "));
            }

            // Behavior for single select
            if (!multi) {
                // Unselect options except the one that will be selected
                if ($target.is("li")) {
                    selectOptions.not($target).removeClass("selected");
                }
                // Select the selected option
                $target.addClass("selected");
                // Set the value to the native select
                $select.val($target.attr("value"));
                // Set the value to the input
                $input.val($target.text().trim());
            }

            // This is used only if Material Design for Bootstrap is selected
            if ($.material) {
                if ($input.val().trim()) {
                    $select.removeClass("empty");
                } else {
                    $select.addClass("empty");
                }
            }

            // Call the callback
            if (this.options.onSelected) {
                this.options.onSelected($target.attr("value"));
            }

        },
        _addOption: function ($ul, $this) {
            // Create the option
            var $option = $("<li></li>");

            // Style the option
            $option.addClass(this.options.optionClass);

            // If the option has some text then transfer it
            if ($this.text()) {
                $option.text($this.text());
            }
                // Otherwise set the empty label and set it as an empty option
            else {
                $option.html("&nbsp;");
            }
            // Set the value of the option
            $option.attr("value", $this.val());

            // Will user be able to remove this option?
            if ($ul.data("select").attr("data-dynamic-opts")) {
                $option.append("<span class=close></span>");
                $option.find(".close").on("click", function () {
                    $option.remove();
                    $this.remove();
                });
            }

            // Ss it selected?
            if ($this.prop("selected")) {
                $option.attr("selected", true);
                $option.addClass("selected");
            }

            // Append option to our dropdown
            if ($ul.find(".dropdownjs-add").length) {
                $ul.find(".dropdownjs-add").before($option);
            } else {
                $ul.append($option);
            }
        },
        destroy: function ($e) {
            $($e).show().removeAttr('data-dropdownjs').next('.dropdownjs').remove();
        }
    };

    $.fn.dropdown = function (params) {
        if (typeof methods[params] == 'function') methods[params](this);
        if (methods[params]) {
            return methods[params].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof params === "object" | !params) {
            return methods.init.apply(this, arguments);
        } else {
            $.error("Method " + params + " does not exists on jQuery.dropdown");
        }
    };

}));

//# sourceMappingURL=material.min.js.map