/* Custom Validation Methods */
jQuery.validator.addMethod("dateGB", function (value, element) { return this.optional(element) || /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/.test(value); }, "Please enter a valid date!"); jQuery.validator.addClassRules("dateGB", { dateGB: true });
jQuery.validator.addMethod("not", function(value, element, target) { return value != target; }, "Value must not be equal to {0}");


//*********************************************
// bigfootMVC
//*********************************************
var bigfootMVC = {

    //******************************************************
    // CONSTANTS
    //******************************************************
    constants: {
        flashId: "bigfootMVC_flash",
        systemErrorId: "bigfootMVC_SystemError",
        systemErrorIFrameId: "bigfootMVC_SystemError_Iframe",
        overlay: {
            effect: "default", exposeColor: "#222", defaultCSSClass: "simple_overlay",

            main: "bigfootMVC_mainOverlay", mainContent: "bigfootMVC_mainOverlayContent", mainMask: "bigfootMVC_mainOverlayMask",
            mainZindex: 10300, mainTrigger: "bigfootMVC_mainOverlayTrigger",

            top: "bigfootMVC_topOverlay", topContent: "bigfootMVC_topOverlayContent", topMask: "bigfootMVC_topOverlayMask",
            topZindex: 10400, topTrigger: "bigfootMVC_topOverlayTrigger",

            message: "bigfootMVC_messageOverlay", messageContent: "bigfootMVC_messageOverlayContent", messageMask: "bigfootMVC_messageOverlayMask",
            messageZindex: 10500, messageTrigger: "bigfootMVC_messageOverlayTrigger"
        }
    },


    //******************************************************
    // CONNECT AJAX
    //******************************************************
    //#region ConnectAjax
    errorHappened: false,

    connectAjax: function () {
        // Clear the flash element
        if (jQuery("#" + bigfootMVC.constants.flashId).length == 0) {
            jQuery("#" + bigfootMVC.constants.flashId).remove();
        }

        // Create html element elements
        var html = "<div id='" + bigfootMVC.constants.flashId + "'></div>";
        html += "<div id='" + bigfootMVC.constants.systemErrorId + "'>";
        html += "<a href='javascript:bigfootMVC.clearGlobalMessage();void(0);' class='close'>CLOSE</a>";
        html += "<iframe id='" + bigfootMVC.constants.systemErrorIFrameId + "'></iframe>";
        html += "</div>";
        jQuery(html).appendTo("body");

        // Clear the flash
        bigfootMVC.clearGlobalMessage();

        // Get the flash element
        var flash = jQuery("#" + bigfootMVC.constants.flashId);

        // Set a request header to flag every ajax request
        jQuery(document).ajaxSend(function (ev, req, options) {
            req.setRequestHeader("ajaxRequest", "true");
        });
        //flash.ajaxSend(function (ev, req, options) {
        //    req.setRequestHeader("ajaxRequest", "true");
        //});

        // Mark the page as updating on every ajax call 
        jQuery(document).ajaxStart(function () {
            bigfootMVC.showGlobalLoading("Updating");
        });

        // ERROR HANDLER. Determine if the error is due to security and redirect to the login page.
        jQuery(document).ajaxError(function (event, request, settings) {
            bigfootMVC.log("AJAX ERROR: " + request.status + " | " + request.responseText);
            bigfootMVC.errorHappened = true;
            if (request.status == 402) {
                window.location = loginurl;
            }
            bigfootMVC.showGlobalError(request.status + ': ' + request.responseText); // request.getResponseHeader("ActionErrorMessage")); //showGlobalError(request.status + ': ' + request.statusText);
        });

        // Hide the Loading message on successful completion
        jQuery(document).ajaxSuccess(function (request, settings) {
            bigfootMVC.clearGlobalMessage();
        });
    },

    //#endregion


    //******************************************************
    // FLASH AND SYSTEM ERROR
    //******************************************************
    //#region Flash And System Error

    clearGlobalMessage: function () {
        jQuery("#" + bigfootMVC.constants.systemErrorIFrameId).attr("src", "about:blank");
        jQuery("#" + bigfootMVC.constants.systemErrorId).hide();

        return jQuery("#" + bigfootMVC.constants.flashId).removeClass("flash_notice flash_alert flash_loading").html("").hide();
    },

    showGlobalError: function (msg) {
        // Inject the html into the iframe
        var iframe = document.getElementById(bigfootMVC.constants.systemErrorIFrameId);
        var doc = (iframe.contentDocument) ? iframe.contentDocument : iframe.contentWindow.document;
        doc.clear();
        doc.open();
        doc.writeln(msg);
        doc.close();

        // show the error
        jQuery("#" + bigfootMVC.constants.systemErrorId).show().centerXY();
    },

    showGlobalNotice: function (msg) {
        var f = bigfootMVC.clearGlobalMessage();
        f.addClass('flash_notice');
        //f.addClass("flash_notice").html(msg).show();
        bigfootMVC.showGlobalMessage(f, msg);
    },

    showGlobalLoading: function (msg) {
        var f = bigfootMVC.clearGlobalMessage();
        f.addClass("flash_loading");
        bigfootMVC.showGlobalMessage(f, msg);
    },

    showGlobalMessage: function (jobj, msg) {
        jobj.animate({ top: jQuery(window).scrollTop() + "px" }, { queue: false, duration: 350 });
        jobj.append(msg).show();
        var w = jobj.width();
    },

    //#endregion


    //******************************************************
    // OVERLAYS
    //******************************************************
    //#region OVERLAYS

    // Creates the overlay information with the proper ids etc.
    getOverlayInfo: function(rootId, zindex){ 
        var rootId = "bfmvc_" + rootId + "Overlay";
        return {
            id: rootId,
            contentId: rootId + "Content",
            maskId: rootId + "Mask",
            zIndex: zindex ? zindex : 200,
            triggerId: rootId + "Trigger",
            clear: function(callback){
                var selector = "#" + this.triggerId;
                if ($(selector).length > 0) $(selector).overlay().close();
                if (callback && (typeof callback) == 'function') { setTimeout(callback, 100); }                
            },
            load: function(html /*, options*/){                
                var options = {
                    id: this.id,
                    contentId: this.contentId,
                    maskId: this.maskId,
                    zIndex: this.zIndex,
                    triggerId: this.triggerId,
                    oneInstance: false,
                    closeOnClick: false
                };
                $.extend(options, arguments[1]);
                bf.loadOverlay(html, options);
            }
        };
    },

    mainOverlay: function(){ return bf.getOverlayInfo("main", 200)},
    topOverlay: function(){return bf.getOverlayInfo("top", 250);},
    messageOverlay: function(){return bf.getOverlayInfo("message", 300);},

    // Clears all overlays in the system
    clearAllOverlays: function (callback) {
        clearTopOverlay(function () {
            clearMessageOverlay(function () {
                clearOverlay(function () {
                    if (callback && (typeof callback) == 'function') { callback(); }
                });
            });
        });
    },

    clearOverlay: function (callback) {
        var selector = "#" + bigfootMVC.constants.overlay.mainTrigger;
        if ($(selector).length > 0) $(selector).overlay().close();
        if (callback && (typeof callback) == 'function') { setTimeout(callback, 100); }
    },

    clearTopOverlay: function (callback) {
        var selector = "#" + bigfootMVC.constants.overlay.topTrigger;
        if ($(selector).length > 0) $(selector).overlay().close();
        if (callback && (typeof callback) == 'function') { setTimeout(callback, 100); }
    },

    clearMessageOverlay: function (callback) {
        var selector = "#" + bigfootMVC.constants.overlay.messageTrigger;
        if ($(selector).length > 0) $(selector).overlay().close();
        if (callback && (typeof callback) == 'function') { setTimeout(callback, 100); }
    },

    /*Load an overlay on top of the message layer*/
    loadMessageOverlay: function (html /*, options*/) {
        var options = {
            id: bigfootMVC.constants.overlay.message,
            contentId: bigfootMVC.constants.overlay.messageContent,
            maskId: bigfootMVC.constants.overlay.messageMask,
            zIndex: bigfootMVC.constants.overlay.messageZindex,
            triggerId: bigfootMVC.constants.overlay.messageTrigger,
            oneInstance: false,
            closeOnClick: false
        };
        $.extend(options, arguments[1]);
        bigfootMVC.loadOverlay(html, options);
    },

    /*Load an overlay on top of the main content for the course layer*/
    loadTopOverlay: function (html /*, options*/) {
        var options = {
            id: bigfootMVC.constants.overlay.top,
            contentId: bigfootMVC.constants.overlay.topContent,
            maskId: bigfootMVC.constants.overlay.topMask,
            zIndex: bigfootMVC.constants.overlay.topZindex,
            triggerId: bigfootMVC.constants.overlay.topTrigger,
            oneInstance: false,
            closeOnClick: false
        };
        $.extend(options, arguments[1]);
        bigfootMVC.loadOverlay(html, options);
    },

    // Load the overlay with some data
    //  Options:
    //      container = container where the overlay will be placed || default = mainContent
    //      id = id to use for the overlay || default = overlay
    //      expose = background effect || default = { color: '#222', loadSpeed: 200, opacity: 0.8 }
    //      effect = custom effect plugin
    //      cssClass = css class to apply to the overlay element || default = bigfootMVC.constants.overlay.defaultCSSClass
    //      closeClass = css class to apply to the overlay close element
    //      zIndex = zindex for the overlay || default = bigfootMVC.constants.overlay.mainZindex
    //      showExpose = true | false determines weather to use an expose || default = true
    //      triggerTop = Top position of the trigger || default = center
    //      triggerLeft = Left position of the trigger || default = center
    //      onLoad = function to be called after the load is complete
    //		onClose = callback function that gets called when the overlay unloads
    //      appendToForm = determines whether to append the overlay to the first form found in the page
    loadOverlay: function (html /*, options*/) {

        var options = {
            id: bigfootMVC.constants.overlay.main,
            contentId: bigfootMVC.constants.overlay.mainContent,
            maskId: bigfootMVC.constants.overlay.mainMask,
            triggerId: bigfootMVC.constants.overlay.mainTrigger,
            cssClass: bigfootMVC.constants.overlay.defaultCSSClass,
            zIndex: bigfootMVC.constants.overlay.mainZindex,
            expose: { color: bigfootMVC.constants.overlay.exposeColor, loadSpeed: 200, opacity: 0.8, closeOnClick: true },
            showExpose: true,
            oneInstance: false,
            effect: bigfootMVC.constants.overlay.effect,
            appendToForm: false,
            fixed: false
        };

        // Merge the caller parameters options parameter
        $.extend(options, arguments[1]);

        // Set the zIndex for the exponse overlay
        options.expose.zIndex = options.zIndex - 1;

        // Set the maskId for the expose
        options.expose.maskId = options.maskId;

        // After the exponse loads: Add mask as a class name
        options.expose.onLoad = function () { $(maskSelector).addClass("mask"); }

        // Get the jquery selectors
        var idSelector = "#" + options.id;
        var contentIdSelector = "#" + options.contentId;
        var maskSelector = "#" + options.expose.maskId;
        var triggerSelector = "#" + options.triggerId;

        // Remove the expose effect if requested
        if (!options.showExpose) options.expose = null;

        // Remove overlay / mask / trigger
        if ($(idSelector).length > 0) $(idSelector).remove();
        if ($(maskSelector).length > 0) $(maskSelector).remove();
        if ($(triggerSelector).length > 0) $(triggerSelector).remove();

        // Create the overlay if it is not found
        var containerSelector = options.appendToForm == true ? "form:eq(0)" : "body";
        $(containerSelector).append("<div id='" + options.id + "' class='" + options.cssClass + " hide overlay'><div id='" + options.contentId + "'></div></div>");

        // Set the z-index
        $(idSelector).css("zIndex", options.zIndex);

        // inject the content
        $(contentIdSelector).html(html);

        // Create the trigger element
        var trigger = $("<a id='" + options.triggerId + "' rel='" + idSelector + "' class='trigger' />").appendTo("body");
        //  Position it
        if (options.triggerTop && options.triggerLeft)
            trigger.css({ position: "absolute", top: options.triggerTop, left: options.triggerLeft });
        else
            trigger.centerXY();

        //  Open the overlay
        trigger.overlay(options).click();

        // Set The class of the close button
        var closeSelector = idSelector + " .close";
        if (options.closeClass) $(closeSelector).removeClass("close").addClass(options.closeClass);

    },

    //#endregion


    S4: function() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    },

    generateGuid: function() {
        return (bigfootMVC.S4() + bigfootMVC.S4() + "-" + bigfootMVC.S4() + "-" + bigfootMVC.S4() + "-" + bigfootMVC.S4() + "-" + bigfootMVC.S4() + bigfootMVC.S4() + bigfootMVC.S4());
    },

    //#region Bootstrap Modal

    //title: string | default: none | modal title 
    //backdrop: boolean or the string 'static' | default: static | Includes a modal-backdrop element. Alternatively, specify static for a backdrop which doesn't close the modal on click.
    //keyboard:	boolean| default: true | Closes the modal when escape key is pressed
    //show:	boolean	| default: true | Shows the modal when initialized
    //footer:	boolean	| default: false | Shows the footer 
    //size: small, large | default: undefined 
    //onClose: on close callback
    showModal: function(data, _options) {
        
        var modalId = bigfootMVC.generateGuid();
        var options = {
            backdrop: 'static',
            keyboard: true,
            show: true,
            footer: false,
            title: ''
        };
        
        // Merge the caller parameters options parameter
        if (_options) {
            $.extend(options, _options);
        }
        var size = "";
        if(options.size === 'small') {
            size = "modal-sm";
        }
        else if(options.size === 'large') {
            size = "modal-lg"; 
        }

        var containerSelector = options.appendToForm == true ? "form:eq(0)" : "body";        
        var modalDiv ='<div id="' + modalId +'" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="' + options.title + '" aria-hidden="true">';
        modalDiv += '   <div class="modal-dialog ' + size + '">';
        modalDiv += '       <div class="modal-content">';
        
        modalDiv += '           <div class="modal-header">';
        modalDiv += '               <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>';
        if (options.title && options.title.trim().length > 0) {
            modalDiv += '               <h4 class="modal-title">' + options.title + '</h4>';
        }
        modalDiv += '           </div>';
        modalDiv += '           <div class="modal-body">';
        modalDiv += '                   ' + data ;      
        modalDiv += '           </div>';

        if (options.footer === true) {
            modalDiv += '           <div class="modal-footer">';
            modalDiv += '               <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>';
            modalDiv += '           </div>';
        }

        modalDiv += '       </div>';
        modalDiv += '   </div>';
        modalDiv += '</div>';

        $(containerSelector).append(modalDiv);        
        $("#" + modalId).modal(options);

        $("#" + modalId).on("hidden.bs.modal", function (e) {            
            $('#' + modalId).remove();
            if (options.onClose && (typeof options.onClose) == 'function') { setTimeout(options.onClose, 100); }
        })
    },
    //#endregion


    //******************************************************
    // HELPER FUNCTIONS
    //******************************************************
    //#region Helper Functions

    log: function (msg) {
        try { console.log(msg); } catch (e) { }
    },

    removePostField: function () {
        for (var i = 0; i < formData.length; i++) {
            if (formData[i].name == fieldName) {
                formData.splice(i, 1);
            }
        }
        return formData;
    },

    getFormData: function () {
        var formData = jQuery("form:eq(0)").formToArray();

        // Remove ASP.NET hidden fields
        bigfootMVC.removePostField(formData, '__VIEWSTATE');
        bigfootMVC.removePostField(formData, '__EVENTTARGET');
        bigfootMVC.removePostField(formData, '__EVENTARGUMENT');

        return formData;
    },

    formatAsJQId: function (id) {
        ///	<summary>
        ///		Formats the passed id to be used as an id selector by jquery
        ///	</summary>
        if (id != null && id != undefined && !id.startsWith("#")) id = "#" + id;
        return id;
    },

    id: function (elementId) {
        ///	<summary>
        ///		Returns a jquery object for the element id
        ///	</summary>
        ///	<param name="elementId" type="string">
        ///		The id of the element without the # sign
        ///	</param>
        ///	<returns type="jQuery" />        

        // If the element passed in is already a jquery object then return it
        if (elementId instanceof jQuery) return elementId;

        // Normalize the id to have the # and then return the jquery object
        return $(bf.formatAsJQId(elementId));
    },

    exists: function (elementId) {
        ///	<summary>
        ///		Returns true if the element is found
        ///	</summary>
        ///	<param name="elementId" type="string">
        ///		The id of the element without the # sign
        ///	</param>
        ///	<returns type="jQuery" />
        if (elementId && elementId.toString().substring(0, 1) != "#") elementId = "#" + elementId;
        return $(elementId).length > 0;
    },

    isVisible: function (elementId) {
        ///	<summary>
        ///		Returns true if the element is found
        ///	</summary>
        ///	<param name="elementId" type="string">
        ///		The id of the element without the # sign
        ///	</param>
        ///	<returns type="jQuery" />
        try {
            if (elementId && elementId.toString().substring(0, 1) != "#") { elementId = "#" + elementId; }
            return $(elementId) && $(elementId).is(":visible");
        }
        catch (e) { log(e); }
    },

    addEnterHandler: function (selector, callback) {
        ///	<summary>
        ///		Attaches an event to the enter key
        ///	</summary>
        ///	<param name="selector" type="string">
        ///		A valid jquery selector
        ///	</param>
        ///	<param name="callback" type="function">
        ///		A valid function to call on enter
        ///	</param>
        ///	<returns type="jQuery" />
        $(selector).bind("keypress", function (e) {
            if (e.keyCode == 13) {
                callback();
                e.preventDefault();
            }
        });
    },

    validateEmail: function (elementValue) {
        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        return emailPattern.test(elementValue);
    },

    //ADD A SELECT OPTION AND SELECT IT 
    AddSelectOption: function (field, key, text, selected) {
        var html = "<option";
        if (selected == true) html += " selected = 'selected'"; 
        html += "></option>";
        jQuery('#' + field).append(jQuery(html).attr("value", key).text(text));
    },

    getUrlParam: function (name) {
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var tmpURL = window.location.href;
        var results = regex.exec(tmpURL);
        if (results == null)
            return "";
        else
            return results[1];
    },
    
    ChangeCallback: function (elemIds, callback) {
        var elems = elemIds.split(",");

        var selString = "";
    
        for (var i = 0; i < elems.length; i++) {
            var id = elems[i].trim();
            if (id.length == 0) continue;

            var selector = !id.startsWith("#") ? "#" + id : id;

            // If not an input box then check if it is a radiolist
            if ($(selector).length == 0) {
                if ($("input:radio[name=" + id + "]").length > 0) {
                    selector = "input:radio[name = " + id + "]";
                }
                else {
                    continue;
                }
            }
        
            // Add it to the list
            if (selString.length > 0) selString += ", ";
            selString += selector;
        }
        $(selString).change(callback);
    },

    //#endregion


    //******************************************************
    // FORM FUNCTIONS
    //******************************************************
    //#region createFormElementSelector | validatePartialForm | ajax

    createFormElementSelector: function (formId) {
        ///	<summary>
        ///		Create a form input elements select for all elements within a particular element id such as a div
        ///	</summary>
        // Make sure the form starts with a # pound sign to use in jQuery
        return "{0} :input, {0} select, {0} textarea".format(bf.formatAsJQId(formId));
    },

    validatePartialForm: function(formId, evt) {
        ///	<summary>
        ///		Validates a set of inputs contained within a certain element id
        ///	</summary>
        var isValid = true;

        // Create the jquery selector
        var selector = bf.createFormElementSelector(formId);

        // Validate each field in the container
        jQuery(selector).each(function(){ 
            if (!$(this).valid()) isValid = false; 
        });

        // When handling validation on an enter handler, rememver to prevent the default enter on the form
        if (!isValid && evt) evt.preventDefault();

        // Return wheter it is valid or not
        return isValid;    
    },
        
    createPartialForm: function (formId) {
        ///	<summary>
        ///		Creates a partial form from an ordinary html element id such as a div. 
        ///     it does this by hooking up validation for the form object including any
        ///     element on the page which has the class .causesValidation. Also when enter is 
        ///     pressed on an input control, the form gets validated and submit of the form is prevented if not valid
        ///	</summary>
        jQuery(formId).parents('form:eq(0)').validate({ onsubmit: false });
        jQuery(formId + ' .causesValidation').click(function () {
            bf.validatePartialForm(formId);
        });
        jQuery(bf.createFormElementSelector(formId)).keydown(function (evt) {
            if (evt.keyCode == 13) { validatePartialForm(formId, evt); }
        });
    },

    serializePartialForm: function (formId) {
        /// <summary>
        ///     Serializes the fields in a partial form and returns them as a json object
        /// </summary>
        var selector = bf.createFormElementSelector(formId);
        //return $(selector).fieldSerialize();       
        var o = {};
        var a = $(selector).serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    },


    ajax: function (url /*, options*/) {
        ///	<summary>
        ///		Validates a set of inputs contained within a certain element id
        ///	</summary>
        ///	<param name="url" type="string">
        ///		The url to post or get from
        ///	</param>
        ///	<param name="options" type="function">
        ///		Set of options and actions to take for the execution of the ajax request
        ///	</param>
        //debugger;

        // Defaults
        var options = {
            url: url,
            clearForm: true,
            formId: "",
            isPartialForm: true,
            validate: true,
            postData: {},
            method: "post", // get | post
            dataType: "html", // html | json | xml | script | jsonp | text
            confirmMessage: "",
            successMessage: "",
            updatePanel: "",
            removeElement: "",            
            callback: "",
            redirectToUrl: "",
            loadUrl: "",
            loadElement: "",
            hideElement: "",
            showElement: "",
            removeClassFromElement: "",
            removeClassFromElementClassNames: "",
            focus: ""            
        };

        // Merge the caller parameters options parameter
        $.extend(options, arguments[1]);
        
        // Serialize the partial form into the postData
        if (options.formId != "") {
            options.formId = bf.formatAsJQId(options.formId);            
            if (options.isPartialForm)
            {
                $.extend(options.postData, bf.serializePartialForm(options.formId));
            }
            else
            {
                $.extend(options.postData, $(options.formId).formToArray());
            }
        }
        
        // Validate
        if (options.formId != "" && options.validate) {
            var isValid = false;
            if (options.isPartialForm) {
                isValid = bf.validatePartialForm(options.formId);
            }
            else {
                isValid = $(options.formId).validate({ onsubmit: false, debug: false }).form();
            }
            if (isValid == false) return;
        }

        // Cofirm if requested
        if (options.confirmMessage != "") {
            if (confirm(options.confirmMessage) == false) return;
        }

        // Execute the request (http://api.jquery.com/jquery.ajax/)
        $.ajax({
            url: options.url,
            type: options.method,
            data: options.postData, 
            success: function (data, status, jqXHR) {
                debugger;
                // Update panel
                if (options.updatePanel != "") {
                    bf.id(options.updatePanel).html(data);
                }

                // Remove element
                if (options.removeElement != "") {
                    bf.id(options.removeElement).remove();
                }

                // Hide element
                if (options.hideElement != "") {
                    bf.id(options.hideElement).hide();
                }

                // Show Element
                if (options.showElement != "") {
                    bf.id(options.showElement).show();
                }

                // Remove Classes
                if (options.removeClassFromElement != "" && options.removeClassFromElementClassNames != "") {
                    bf.id(options.removeClassFromElement).removeClass(options.removeClassFromElementClassNames);
                }
                
                // Clear the form and associated validation
                if (options.clearForm == true && options.formId != "") {
                    var fieldSelector = bf.createFormElementSelector(options.formId);
                    $(fieldSelector).clearFields();
                    $(options.formId + " .error:not(label.error)").removeClass("error");
                    $(options.formId + " label.error").remove();
                }

                // Set the focus
                if (options.focus != "") {
                    bf.id(options.focus).focus();
                }

                // Load url into element
                if (options.loadUrl != "" && options.loadElement != "") {
                    bf.id(options.loadElement).load(options.loadUrl).show();
                }

                // Success message
                if (options.successMessage != "") {
                    alert(options.successMessage);
                }

                // Callback function
                if (options.callback != "") {
                    (options.callback)(data, status, jqXHR);
                }

                // Redirect to url
                if (options.redirectToUrl != "" && !bf.errorHappened) {
                    window.location = options.redirectToUrl;
                }

            }
            //,
            //error: function(jqXHR, textStatus, errorThrown){

            //}
        });

        // Return false just in case inside a button in order to cancel the button's default event
        return false;
    },

    //#endregion
    
    //#region submitForm | submitParentForm

    submitParentForm: function (elem /*, options*/) {
        ///<summary>
        /// Submits the parent form of a particular elemnt
        ///</summary>  
        debugger;
        bf.submitForm($(elem).parents("form:eq(0)"), arguments[1]);
    },

    submitForm: function (formId /*, options*/) {
        ///<summary>
        /// Submits the form with the id specified. You may also pass a jquery object with the form selected
        ///</summary>        
        debugger;
        var options = {            
            validate: true,
            validatePartialFormId: "",
            viaAjax: false,
            ajaxPostUrl: "",
            updatePanel: "", // may be specified when submitting the form via ajax,
            ajaxOptions: {} // may be used to specify additional ajax form options
        };

        // Merge the caller parameters options parameter
        $.extend(options, arguments[1]);

        // Get a reference to the form        
        var $form = bf.id(formId);

        // Validate
        if (options.validate) {
            var isValid = true;

            // Validate a partial form as supposed to a regular form
            if (options.validatePartialFormId != "") {
                isValid = bf.validatePartialForm(options.validatePartialFormId);
            }
            else {
                isValid = $form.validate({ onsubmit: false, debug: false }).form();
            }
            // Cancel if not valid
            if (!isValid) { return; }                
        }
        

        // Submit the form
        if (options.viaAjax) {
            // Create the ajax form
            var ajaxOptions = {
                url: options.ajaxPostUrl,
                target: options.updatePanel
            };
            // Merge additional options which were sent in
            $.extend(ajaxOptions, options.ajaxOptions);
            // Submit via ajax
            $form.ajaxSubmit(ajaxOptions);
        }
        else {
            // Trigger the parent form submit
           $form.trigger("submit");
        }

    },

    //#endregion


    //******************************************************
    // FORM FUNCTIONS
    //******************************************************
    loadUrl: function (url, elementId, toggle) {

        if (toggle && bf.isVisible(elementId))
        {
            bf.id(elementId).html("").hide();
            return;
        }
        bf.id(elementId).load(url).show();

    }
    
};



//#region String Extensions
String.prototype.format = function () {
    var s = this,
            i = arguments.length;
    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};
String.prototype.startsWith = function(str) { return (this.match("^" + str) == str); };
String.prototype.endsWith = function(str) { return (this.match(str + "$") == str); };
String.prototype.trim = function () { return (this.replace( /^[\s\xA0]+/ , "").replace( /[\s\xA0]+$/ , "")); };
String.prototype.ltrim = function() { return this.replace( /^\s+/ , ""); };
String.prototype.rtrim = function() { return this.replace( /\s+$/ , ""); };
String.prototype.contains = function (it) { return this.indexOf(it) != -1; };
//#endregion


//#region dateFormat

/* DATE FORMAT */
/*
* Date Format 1.2.3: http://blog.stevenlevithan.com/archives/date-time-format
* (c) 2007-2009 Steven Levithan <stevenlevithan.com>
* MIT license
*
* Includes enhancements by Scott Trenda <scott.trenda.net>
* and Kris Kowal <cixar.com/~kris.kowal/>
*
* Accepts a date, a mask, or a date and a mask.
* Returns a formatted version of the given date.
* The date defaults to the current date/time.
* The mask defaults to dateFormat.masks.default.
*/
var dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len) {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
        var dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            m = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            M = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.i18n.dayNames[D],
                dddd: dF.i18n.dayNames[D + 7],
                m: m + 1,
                mm: pad(m + 1),
                mmm: dF.i18n.monthNames[m],
                mmmm: dF.i18n.monthNames[m + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                M: M,
                MM: pad(M),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
} ();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

// Internationalization strings
dateFormat.i18n = {
    dayNames: [
        "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
};

//#endregion


//#region CheckIf (Checks IsDate, IsNumber, IsEmptyString etc.)

var CheckIf = {

    IsDate: function (d) {
        //Validate that the same date as the one entered
        //Format expected is mm/dd/yyyy
        var newDate = new Date(d);
        var isValidDate = !isNaN(newDate) && !CheckIf.IsEmptyString(d);
        if(isValidDate){
            //check the values entered are the ones received
            //This will fix the issue that makes javascript convert invalid dates
            var splitDate = d.split("/");
            if (
                    splitDate.length != 3 
                    ||
                    (
                        (newDate.getDate() != splitDate[0] || newDate.getMonth()+1 != splitDate[1]) 
                        && 
                        (newDate.getDate() != splitDate[1] || newDate.getMonth()+1 != splitDate[0])
                    )
                    ||
                    newDate.getFullYear() != splitDate[2]
                ){
                    isValidDate= false;
                }


        }
        return isValidDate;
    },

    IsNumber: function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    },

    IsEmptyString: function (s) {
        return !s || s.trim().length == 0;
    }

};

//#endregion


//#region "val function: Returns the value of any input element as well as spans and divs"
function val(elemId, setValue) {
    // Try to get a field by it's id
    var $elem = bfmvc.id(elemId);

    // Try to get a radio list if not found
    if ($elem.length == 0)  $elem = $('input:radio[name=' + elemId + ']');

    // Determine if it is a radio
    var isRadio = $elem.is(":radio");
    
    // Determine if it is a checkbox
    var isCheckbox = $elem.is(":checkbox");
    
    // Check if found
    if ($elem.length == 0) return "";

    if (isRadio) {
        //$elem.removeAttr("checked");
        if(setValue != undefined) $elem.filter("[value='" + setValue + "']").attr("checked", "checked");
        return $elem.filter(":checked").val();
    }
    else if (isCheckbox) {
        if (setValue != undefined) $elem.attr("checked", setValue);
        return $elem.is(":checked");
    }
    else {
        // Figure out if you are trying to get or set the value of something other than input, textarea, or select
        var tagName = $elem.get(0).tagName.toLowerCase();
        if (tagName != "input" && tagName != "textarea" && tagName != "select")
        {
            if (setValue != undefined) $elem.text(setValue);
            return $elem.text();
        }
        else {
            if (setValue != undefined) $elem.val(setValue);
            return $elem.val();
        }
    }
};
//#endregion


// Alias bigfootMVC as bfmvc and bf
var bfmvc = bigfootMVC;
var bf = bigfootMVC;

// Connect Ajax once it has been loaded and jquery has been loaded
jQuery(function () {

    if (!window.DoNotConnectBigfootMVCAjax){
        bf.connectAjax();
    }

});



