/*jquery.cascading.js */
/*
* JQCascade: A jQuery cascading drop down
* version: 1.0.0 (10/4/2010)
* 
* Dual license under MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*
* Copyright 2010 Ryan Wright
* http://www.ryanmwright.com
*
*/

;(function ($) {
    $.cascading = function (element, options) {

        this.init = function (element, options) {

            // Get the options
            var cascadingOptions = $.extend({}, $.cascading.defaultOptions, options);

            // Set the options on the DOM element
            $(element).data('cascadingOptions', cascadingOptions);

            // Get the parent
            var parent = $("#" + cascadingOptions.parentDropDownId);

            // Set the parent on the DOM element
            $(element).data("cascadingParent", parent);

            $(element).data("noSelectionValue", cascadingOptions.noSelectionValue);

            if(cascadingOptions.disableUntilParentSelected) {
                $(element).attr('disabled', 'disabled');
            }

            // Validate
            doValidate(element, cascadingOptions, parent);

            // Target element must be a select list
            element.each(function (index, value) {
                // Attach this to the parent's list of children
                var children = parent.data('cascadingChildren');
                if (children == null) {
                    children = new Array();
                }
                children.push(value);
                parent.data('cascadingChildren', children)
            });

            // When the parent changes invoke the ajax call
            $(parent).change(function () {

                var prnt = $(this);

                // Disable the list and invoke the ajax call
                $.each($(this).data("cascadingChildren"), function (index, value) {
                    if (prnt.val() != prnt.data("noSelectionValue")) {
                        value.disabled = true;
                        disableChildrenBeforePost($(value));
                        // Invoke the request
                        eval("$.ajax({ type: 'POST', url: '" + $(value).data("cascadingOptions").dataUrl + "', context: this, contentType: 'application/x-www-form-urlencoded', data: { " + $(value).data("cascadingOptions").serviceParameterName + " : '" + prnt.val() + "' }, dataType: 'json', success: dataSuccess, error: dataError });");
                    } else {
                        // If no item is selected do an inherited chained cascade through all children
                        inheritedCascade(parent.data('cascadingOptions').chainedCascadeMode, parent);
                    }
                });

            });
        }

        // Initialize
        this.init(element, options);
    }

    function disableChildrenBeforePost(elem) {
        // Enable the list and reset the dropdown's length
        var children = elem.data('cascadingChildren');

        if (children != null) {
            $.each(children, function (index, value) {
                if (!$(value).attr('disabled')) {
                    $(value).attr('disabled', 'disabled');
                    $(value).data('ajaxDisabled', 'true');
                    disableChildrenBeforePost($(value));
                }
            });
        }
    }

    function enableChildrenAfterPost(elem) {
        // Enable the list and reset the dropdown's length
        var children = elem.data('cascadingChildren');

        if (children != null) {
            $.each(children, function (index, value) {
                if ($(value).data('ajaxDisabled') != undefined) {
                    $(value).removeAttr('disabled');
                    $(value).data('ajaxDisabled', undefined)
                    enableChildrenAfterPost($(value));
                }
            });
        }
    }

    // Private function: performs cascade where all children inherit the cascading option from their parent
    function inheritedCascade(chainedCascadeMode, elem) {

        if (chainedCascadeMode == 'ClearChildren') {                      // Clear all children to empty
            // Enable the list and reset the dropdown's length
            var children = elem.data('cascadingChildren');

            if (children != null) {
                $.each(children, function (index, value) {
                    value.length = 0;
                    inheritedCascade(chainedCascadeMode, $(value));
                });
            }
        } else if (chainedCascadeMode == 'ClearChildrenToNoSelection') {         // Clear all children to the no selection
            // Enable the list and reset the dropdown's length
            var children = elem.data('cascadingChildren');

            if (children != null) {
                $.each(children, function (index, value) {
                    value.length = 0;
                    $("<option>").attr("value", $(value).data("cascadingOptions").noSelectionValue).text($(value).data("cascadingOptions").noSelectionText).appendTo("#" + value.id);
                    inheritedCascade(chainedCascadeMode, $(value));
                });
            }
        } else if (chainedCascadeMode == 'ClearAndDisableChildrenToNoSelection') {         // Clear all children to the no selection
            // Enable the list and reset the dropdown's length
            var children = elem.data('cascadingChildren');

            if (children != null) {
                $.each(children, function (index, value) {
                    value.length = 0;
                    $("<option>").attr("value", $(value).data("cascadingOptions").noSelectionValue).text($(value).data("cascadingOptions").noSelectionText).appendTo("#" + value.id);
                    $(value).attr('disabled', 'disabled');
                    inheritedCascade(chainedCascadeMode, $(value));
                });
            }
        } else if (chainedCascadeMode == 'HideChildren') {         // Clear all children to the no selection
            // Enable the list and reset the dropdown's length
            var children = elem.data('cascadingChildren');

            if (children != null) {
                $.each(children, function (index, value) {
                    $(value).hide();
                    inheritedCascade(chainedCascadeMode, $(value));
                });
            }
        }

    }

    // Private function: Validate element against parent and cascading options
    function doValidate(element, cascadingOptions, parent) {

        if (cascadingOptions.dataUrl == '') {
            var msg = "dataUrl must be defined for $().cascading() dropdown \"";
            alert(msg);
            throw msg;
        }

        if (cascadingOptions.parentDropDownId == '') {
            var msg = "parentDropDown must be defined for $().cascading() dropdown \"";
            alert(msg);
            throw msg;
        }

        if (parent.length < 1) {
            var msg = "parentDropDown does not exist for $().cascading() dropdown\"";
            alert(msg);
            throw msg;
        }

        // Parent must be a select list
        parent.each(function (index, value) {
            if (value.tagName != "SELECT") {
                var msg = "parentDropDown is not a select for $().cascading() dropdown\"";
                alert(msg);
                throw msg;
            }
        });

        // Target element must be a select list
        element.each(function (index, value) {
            if (value.tagName != "SELECT") {
                var msg = "DOM element \"" + value.id + "\" is not a select element and cannot be used with $().cascade()";
                alert(msg);
                throw msg;
            }

            parent.each(function (i, v) {
                if (v.id == value.id) {
                    var msg = "Circular reference detected with DOM ID \"" + value.id + "\" in $().cascade(). Are you pointing a parent to itself as a child?";
                    alert(msg);
                    throw msg;
                }
            });
        });
    }

    // Private function: occurrs when the ajax call fails
    function dataError(message) {

        // Notify error
        alert('Call failed!');

        // Re-enable the drop downs
        $(this).each(function (index, value) {
            value.disabled = false;
            enableChildrenAfterPost($(value));
        });
    }


    // Private function: occurrs when the ajax call is successful
    function dataSuccess(message) {

        // Enable the list and reset the dropdown's length
        $(this).each(function (index, value) {
            value.length = 0;
            value.disabled = false;
            enableChildrenAfterPost($(value));
        });

        var item = $(this);

        item.each(function (index, value) {
            $("<option>").attr("value", $(value).data("cascadingOptions").noSelectionValue).text($(value).data("cascadingOptions").noSelectionText).appendTo("#" + value.id);
        });

        // For each message add the corresponding option attribute
        $.each(message, function (i, v) {
            item.each(function (index, value) {
                $("<option>").attr("value", v.Value).text(v.Text).appendTo("#" + value.id);
            });
        });

        item.removeAttr('disabled', 'disabled');
        item.show();

        if ($(this).data('cascadingOptions').chainedCascadeMode == 'CascadeChildren') {                           // Trigger a lookup on all children
            // Trigger the change event, which will update all the children
            $(this).trigger('change');
        } else {
            inheritedCascade($(this).data('cascadingOptions').chainedCascadeMode, $(this));
        }
    }

    // Holds all the default options for the drop down
    $.cascading.defaultOptions = {
        dataUrl: '',
        serviceParameterName: 'val',
        valueName: 'value',
        textName: 'text',
        parentDropDownId: '',
        hideUntilParentSelected: 'false',
        disableUntilParentSelected: 'true',
        chainedCascadeMode: 'ClearAndDisableChildrenToNoSelection', // 'CascadeChildren', 'ClearChildren', 'ClearChildrenToNoSelection', 'ClearAndDisableChildrenToNoSelection', 'HideChildren', 'None'
        noSelectionValue: '0',
        noSelectionText: '[Select]'
    }

    $.fn.cascading = function (options) {
        return this.each(function () {
            (new $.cascading($(this), options));
        });
    };

})(jQuery)