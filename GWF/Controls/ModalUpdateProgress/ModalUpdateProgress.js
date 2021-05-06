Sys.UI._ModalUpdateProgress = function Sys$UI$_ModalUpdateProgress(element) {
    Sys.UI._ModalUpdateProgress.initializeBase(this,[element]);
    this._backgroundCssClass = null;
    this._cancelControlID = null;    
    this._backgroundElement = null;
    this._foregroundElement = null;
    this._cancelHandler = null;
    this._scrollHandler = null;
    this._resizeHandler = null;
    this._windowHandlersAttached = false;    
    this._beginRequestHandlerDelegate = null;
    this._startDelegate = null;
    this._endRequestHandlerDelegate = null;
    this._pageRequestManager = null;
    this._timerCookie = null;    
    this._saveTabIndexes = new Array();
    this._saveDisableSelect = new Array();
    this._tagWithTabIndex = new Array('A','AREA','BUTTON','INPUT','OBJECT','SELECT','TEXTAREA','IFRAME');
    this._disableTabsCalled = false;
}
    function Sys$UI$_ModalUpdateProgress$get_backgroundCssClass() {
        /// <value type="String"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._backgroundCssClass;
    }
    function Sys$UI$_ModalUpdateProgress$set_backgroundCssClass(value) {
        this._backgroundCssClass = value;
    }
    function Sys$UI$_ModalUpdateProgress$get_cancelControlID() {
        /// <value type="String"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._cancelControlID;
    }
    function Sys$UI$_ModalUpdateProgress$set_cancelControlID(value) {
        this._cancelControlID = value;
    }
    function Sys$UI$_ModalUpdateProgress$_attachPopup() {
        if (!this._scrollHandler)
        {
            this._scrollHandler = Function.createDelegate(this, this._onLayout);
        }
        if (!this._resizeHandler)
        {
            this._resizeHandler = Function.createDelegate(this, this._onLayout);
        }
        $addHandler(window, 'resize', this._resizeHandler);
        $addHandler(window, 'scroll', this._scrollHandler);
        this._windowHandlersAttached = true;
    }
    function Sys$UI$_ModalUpdateProgress$_detachPopup() {
        if (this._windowHandlersAttached) {
            if (this._scrollHandler) {
                // && window._events['scroll'].length > 0
                $removeHandler(window, 'scroll', this._scrollHandler);
            }
            if (this._resizeHandler) {
                //&& window._events['resize'].length > 0
                $removeHandler(window, 'resize', this._resizeHandler);
            }
        }
    }
    function Sys$UI$_ModalUpdateProgress$_onCancel(e) {
        var element = $get(this._cancelControlID);
        if (element && !element.disabled) {
            if (this._pageRequestManager !== null) {
                this._pageRequestManager.abortPostBack();
            }
            this._hide();
            e.preventDefault();
            return false;
        }
    }
    function Sys$UI$_ModalUpdateProgress$_onLayout(e) {
        this._layout();
    }
    function Sys$UI$_ModalUpdateProgress$_layout() {
        var scrollLeft = (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
        var scrollTop = (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
        var clientWidth;
        if (window.innerWidth) {
            clientWidth = ((Sys.Browser.agent === Sys.Browser.Safari) ? window.innerWidth : Math.min(window.innerWidth, document.documentElement.clientWidth));
        } else {
            clientWidth = document.documentElement.clientWidth;
        }
        var clientHeight;
        if (window.innerHeight) {
            clientHeight = ((Sys.Browser.agent === Sys.Browser.Safari) ? window.innerHeight : Math.min(window.innerHeight, document.documentElement.clientHeight));
        } else {
            clientHeight = document.documentElement.clientHeight;
        }
        this._backgroundElement.style.left = scrollLeft+'px';
        this._backgroundElement.style.top = scrollTop+'px';
        this._backgroundElement.style.width = clientWidth+'px';
        this._backgroundElement.style.height = clientHeight+'px';
        this._foregroundElement.style.left = scrollLeft+((clientWidth-this._foregroundElement.offsetWidth)/2)+'px';
        this._foregroundElement.style.top = scrollTop+((clientHeight-this._foregroundElement.offsetHeight)/2)+'px';
    }
    function Sys$UI$_ModalUpdateProgress$_show() {
        this._attachPopup();
        this._backgroundElement.style.display = '';
        if (this._dynamicLayout) {
            this._foregroundElement.style.display = 'block';
        } else {
            this._foregroundElement.style.visibility = 'visible';
        }
        this._disableTabs();
        this._layout();
        this._layout();
    }
    function Sys$UI$_ModalUpdateProgress$_disableTabs() {
        if (!this._disableTabsCalled)
        {
            var i = 0;
            var tagElements;
            var tagElementsInPopUp = new Array();
            Array.clear(this._saveTabIndexes);
            for (var j = 0; j < this._tagWithTabIndex.length; j++) {
                tagElements = this._foregroundElement.getElementsByTagName(this._tagWithTabIndex[j]);
                for (var k = 0 ; k < tagElements.length; k++) {
                    tagElementsInPopUp[i] = tagElements[k];
                    i++;
                }
            }
            i = 0;
            for (var j = 0; j < this._tagWithTabIndex.length; j++) {
                tagElements = document.getElementsByTagName(this._tagWithTabIndex[j]);
                for (var k = 0 ; k < tagElements.length; k++) {
                    if (Array.indexOf(tagElementsInPopUp, tagElements[k]) == -1)  {
                        this._saveTabIndexes[i] = {tag: tagElements[k], index: tagElements[k].tabIndex};
                        tagElements[k].tabIndex="-1";
                        i++;
                    }
                }
            }
            i = 0;
            if ((Sys.Browser.agent === Sys.Browser.InternetExplorer) && (Sys.Browser.version < 7)) {
                var tagSelectInPopUp = new Array();
                tagElements = this._foregroundElement.getElementsByTagName('SELECT');
                for (var k = 0 ; k < tagElements.length; k++) {
                    tagSelectInPopUp[i] = tagElements[k];
                    i++;
                }
                i = 0;
                Array.clear(this._saveDisableSelect);
                tagElements = document.getElementsByTagName('SELECT');
                for (var k = 0 ; k < tagElements.length; k++) {
                    if (Array.indexOf(tagSelectInPopUp, tagElements[k]) == -1)  {
                        this._saveDisableSelect[i] = {tag: tagElements[k], visib: this._getCurrentStyle(tagElements[k], 'visibility')} ;
                        tagElements[k].style.visibility = 'hidden';
                        i++;
                    }
                }
            }
	        this._disableTabsCalled = true;
	    }
    }
    function Sys$UI$_ModalUpdateProgress$_restoreTabs() {
        if (this._disableTabsCalled)
        {
            for (var i = 0; i < this._saveTabIndexes.length; i++) {
                this._saveTabIndexes[i].tag.tabIndex = this._saveTabIndexes[i].index;
            }
            if ((Sys.Browser.agent === Sys.Browser.InternetExplorer) && (Sys.Browser.version < 7)) {
                for (var k = 0 ; k < this._saveDisableSelect.length; k++) {
                    this._saveDisableSelect[k].tag.style.visibility = this._saveDisableSelect[k].visib;
                }
            }
	        this._disableTabsCalled = false;
	    }
    }
    function Sys$UI$_ModalUpdateProgress$_hide() {
        this._backgroundElement.style.display = 'none';
        this._foregroundElement.style.display = 'none';
        this._restoreTabs();
        this._detachPopup();
    }
    function Sys$UI$_ModalUpdateProgress$_handleBeginRequest(sender, arg) {
        var curElem = arg.get_postBackElement();
        var showProgress = !this._associatedUpdatePanelId; 
        while (!showProgress && curElem) {
            if (curElem.id && this._associatedUpdatePanelId === curElem.id) {
                showProgress = true; 
            }
            curElem = curElem.parentNode; 
        } 
        if (showProgress) {
            this._timerCookie = window.setTimeout(this._startDelegate, this._displayAfter);
        }
    }
    function Sys$UI$_ModalUpdateProgress$_startRequest() {
        if (this._pageRequestManager.get_isInAsyncPostBack()) {
            this._show();
        }
        this._timerCookie = null;
    }
    function Sys$UI$_ModalUpdateProgress$_handleEndRequest(sender, arg) {
        this._hide();
        if (this._timerCookie) {
            window.clearTimeout(this._timerCookie);
            this._timerCookie = null;
        }
    }
    function Sys$UI$_ModalUpdateProgress$_getCurrentStyle(element, attribute, defaultValue) {
        var currentValue = null;
        if (element) {
            if (element.currentStyle) {
                currentValue = element.currentStyle[attribute];
            } else if (document.defaultView && document.defaultView.getComputedStyle) {
                var style = document.defaultView.getComputedStyle(element, null);
                if (style) {
                    currentValue = style[attribute];
                }
            }
            
            if (!currentValue && element.style.getPropertyValue) {
                currentValue = element.style.getPropertyValue(attribute);
            }
            else if (!currentValue && element.style.getAttribute) {
                currentValue = element.style.getAttribute(attribute);
            }       
        }
        
        if ((!currentValue || currentValue == "" || typeof(currentValue) === 'undefined')) {
            if (typeof(defaultValue) != 'undefined') {
                currentValue = defaultValue;
            }
            else {
                currentValue = null;
            }
        }   
        return currentValue;     
    }
    function Sys$UI$_ModalUpdateProgress$dispose() {
        this._detachPopup();
        this._scrollHandler = null;
        this._resizeHandler = null;
        if (this._cancelHandler && $get(this._cancelControlID)) {
            $removeHandler($get(this._cancelControlID), 'click', this._cancelHandler);
            this._cancelHandler = null;
        }
        if (this._pageRequestManager !== null) {
            this._pageRequestManager.remove_beginRequest(this._beginRequestHandlerDelegate);
            this._pageRequestManager.remove_endRequest(this._endRequestHandlerDelegate);
        }
        Sys.UI._ModalUpdateProgress.callBaseMethod(this,"dispose");
    }
    function Sys$UI$_ModalUpdateProgress$initialize() {
        Sys.UI._ModalUpdateProgress.callBaseMethod(this, 'initialize');

        this._foregroundElement = this.get_element();
        this._backgroundElement = document.createElement('div');
        this._backgroundElement.style.display = 'none';
        this._backgroundElement.style.position = 'absolute';
        this._backgroundElement.style.zIndex = 100000;
        if (this._backgroundCssClass) {
            this._backgroundElement.className = this._backgroundCssClass;
        }
        this._foregroundElement.parentNode.appendChild(this._backgroundElement);
        this._foregroundElement.style.display = 'none';
        this._foregroundElement.style.position = 'absolute';
        this._foregroundElement.style.zIndex = this._getCurrentStyle(this._backgroundElement, 'zIndex', this._backgroundElement.style.zIndex) + 1;

        if (this._cancelControlID) {
            this._cancelHandler = Function.createDelegate(this, this._onCancel);
            $addHandler($get(this._cancelControlID), 'click', this._cancelHandler);
        }

        this._scrollHandler = Function.createDelegate(this, this._onLayout);
        this._resizeHandler = Function.createDelegate(this, this._onLayout);
        
    	this._beginRequestHandlerDelegate = Function.createDelegate(this, this._handleBeginRequest);
    	this._endRequestHandlerDelegate = Function.createDelegate(this, this._handleEndRequest);
    	this._startDelegate = Function.createDelegate(this, this._startRequest);
    	if (Sys.WebForms && Sys.WebForms.PageRequestManager) {
           this._pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
    	}
    	if (this._pageRequestManager !== null ) {
               	    this._pageRequestManager.add_beginRequest(this._beginRequestHandlerDelegate);
    	    this._pageRequestManager.add_endRequest(this._endRequestHandlerDelegate);
    	}
    }

Sys.UI._ModalUpdateProgress.prototype = {
    get_backgroundCssClass: Sys$UI$_ModalUpdateProgress$get_backgroundCssClass,
    set_backgroundCssClass: Sys$UI$_ModalUpdateProgress$set_backgroundCssClass,
    get_cancelControlID: Sys$UI$_ModalUpdateProgress$get_cancelControlID,
    set_cancelControlID: Sys$UI$_ModalUpdateProgress$set_cancelControlID,
    _attachPopup: Sys$UI$_ModalUpdateProgress$_attachPopup,
    _detachPopup: Sys$UI$_ModalUpdateProgress$_detachPopup,
    _onCancel: Sys$UI$_ModalUpdateProgress$_onCancel,
    _onLayout: Sys$UI$_ModalUpdateProgress$_onLayout,
    _layout: Sys$UI$_ModalUpdateProgress$_layout,
    _show: Sys$UI$_ModalUpdateProgress$_show,
    _disableTabs: Sys$UI$_ModalUpdateProgress$_disableTabs,
    _restoreTabs: Sys$UI$_ModalUpdateProgress$_restoreTabs,
    _hide: Sys$UI$_ModalUpdateProgress$_hide,
    _handleBeginRequest: Sys$UI$_ModalUpdateProgress$_handleBeginRequest,
    _startRequest: Sys$UI$_ModalUpdateProgress$_startRequest,
    _handleEndRequest: Sys$UI$_ModalUpdateProgress$_handleEndRequest,
    _getCurrentStyle: Sys$UI$_ModalUpdateProgress$_getCurrentStyle,
    dispose: Sys$UI$_ModalUpdateProgress$dispose,
    initialize: Sys$UI$_ModalUpdateProgress$initialize
}
Sys.UI._ModalUpdateProgress.registerClass('Sys.UI._ModalUpdateProgress', Sys.UI._UpdateProgress);
