var $dp, WdatePicker;
(function () {
    var _ = {
        $wdate: true,
        $dpPath: "",
        //$crossFrame: true,
        $crossFrame: false,
        doubleCalendar: false,
        autoUpdateOnChanged: false,
        position: {},
        lang: "auto",
        skin: "default",
        dateFmt: "yyyy-MM-dd",
        realDateFmt: "yyyy-MM-dd",
        realTimeFmt: "HH:mm:ss",
        realFullFmt: "%Date %Time",
        minDate: "1900-01-01 00:00:00",
        maxDate: "2099-12-31 23:59:59",
        startDate: "",
        alwaysUseStartDate: false,
        yearOffset: 1911,
        firstDayOfWeek: 0,
        isShowWeek: false,
        highLineWeekDay: true,
        isShowClear: true,
        isShowToday: true,
        isShowOK: true,
        isShowOthers: true,
        readOnly: false,
        errDealMode: 0,
        autoPickDate: null,
        qsEnabled: true,

        specialDates: null,
        specialDays: null,
        disabledDates: null,
        disabledDays: null,
        opposite: false,
        onpicking: null,
        onpicked: null,
        onclearing: null,
        oncleared: null,
        ychanging: null,
        ychanged: null,
        Mchanging: null,
        Mchanged: null,
        dchanging: null,
        dchanged: null,
        Hchanging: null,
        Hchanged: null,
        mchanging: null,
        mchanged: null,
        schanging: null,
        schanged: null,
        eCont: null,
        vel: null,
        errMsg: "",
        quickSel: [],
        has: {}
    };
    WdatePicker = U;
    var X = window, O = "document", J = "documentElement", C = "getElementsByTagName", V, A, T, I, b;
    switch (navigator.appName) {
        case "Microsoft Internet Explorer":
            T = true;
            break;
        case "Opera":
            b = true;
            break;
        default:
            I = true;
            break
    }
    A = L();
    if (_.$wdate)
        M(A + "skin/WdatePicker.css");
    V = X;
    if (_.$crossFrame) {
        try {
            while (V.parent[O] != V[O]
                    && V.parent[O][C]("frameset").length == 0)
                V = V.parent
        } catch (P) {
        }
    }
    if (!V.$dp)
        V.$dp = {
            ff: I,
            ie: T,
            opera: b,
            el: null,
            win: X,
            status: 0,
            defMinDate: _.minDate,
            defMaxDate: _.maxDate,
            flatCfgs: []
        };
    B();
    if ($dp.status == 0)
        Z(X, function () {
            U(null, true)
        });
    if (!X[O].docMD) {
        E(X[O], "onmousedown", D);
        X[O].docMD = true
    }
    if (!V[O].docMD) {
        E(V[O], "onmousedown", D);
        V[O].docMD = true
    }
    E(X, "onunload", function () {
        if ($dp.dd)
            Q($dp.dd, "none")
    });
    function B() {
        V.$dp = V.$dp || {};
        obj = {
            $: function ($) {
                return (typeof $ == "string") ? X[O].getElementById($) : $
            },
            $D: function ($, _) {
                return this.$DV(this.$($).value, _)
            },
            $DV: function (_, $) {
                if (_ != "") {
                    this.dt = $dp.cal.splitDate(_, $dp.cal.dateFmt);
                    if ($)
                        for (var A in $) {
                            if (this.dt[A] === undefined)
                                this.errMsg = "invalid property:" + A;
                            this.dt[A] += $[A]
                        }
                    if (this.dt.refresh())
                        return this.dt
                }
                return ""
            },
            show: function () {
                Q(this.dd, "block")
            },
            hide: function () {
                Q(this.dd, "none")
            },
            attachEvent: E
        };
        for (var $ in obj)
            V.$dp[$] = obj[$];
        $dp = V.$dp
    }
    function E(A, $, _) {
        if (T)
            A.attachEvent($, _);
        else {
            var B = $.replace(/on/, "");
            _._ieEmuEventHandler = function ($) {
                return _($)
            };
            A.addEventListener(B, _._ieEmuEventHandler, false)
        }
    }
    function L() {
        var _, A, $ = X[O][C]("script");
        for (var B = 0; B < $.length; B++) {
            _ = $[B].src.substring(0, $[B].src.toLowerCase()
                    .indexOf("wdatepicker.js"));
            A = _.lastIndexOf("/");
            if (A > 0)
                _ = _.substring(0, A + 1);
            if (_)
                break
        }
        return _
    }
    function F(F) {
        var E, C;
        if (F.substring(0, 1) != "/" && F.indexOf("://") == -1) {
            E = V.location.href;
            C = location.href;
            if (E.indexOf("?") > -1)
                E = E.substring(0, E.indexOf("?"));
            if (C.indexOf("?") > -1)
                C = C.substring(0, C.indexOf("?"));
            var G, I, $ = "", D = "", A = "", J, H, B = "";
            for (J = 0; J < Math.max(E.length, C.length); J++) {
                G = E.charAt(J).toLowerCase();
                I = C.charAt(J).toLowerCase();
                if (G == I) {
                    if (G == "/")
                        H = J
                } else {
                    $ = E.substring(H + 1, E.length);
                    $ = $.substring(0, $.lastIndexOf("/"));
                    D = C.substring(H + 1, C.length);
                    D = D.substring(0, D.lastIndexOf("/"));
                    break
                }
            }
            if ($ != "")
                for (J = 0; J < $.split("/").length; J++)
                    B += "../";
            if (D != "")
                B += D + "/";
            F = E.substring(0, E.lastIndexOf("/") + 1) + B + F
        }
        _.$dpPath = F
    }
    function M(A, $, B) {
        var D = X[O][C]("HEAD").item(0), _ = X[O].createElement("link");
        if (D) {
            _.href = A;
            _.rel = "stylesheet";
            _.type = "text/css";
            if ($)
                _.title = $;
            if (B)
                _.charset = B;
            D.appendChild(_)
        }
    }
    function Z($, _) {
        E($, "onload", _)
    }
    function G($) {
        $ = $ || V;
        var A = 0, _ = 0;
        while ($ != V) {
            var D = $.parent[O][C]("iframe");
            for (var F = 0; F < D.length; F++) {
                try {
                    if (D[F].contentWindow == $) {
                        var E = W(D[F]);
                        A += E.left;
                        _ += E.top;
                        break
                    }
                } catch (B) {
                }
            }
            $ = $.parent
        }
        return {
            "leftM": A,
            "topM": _
        }
    }
    function W(F) {
        if (F.getBoundingClientRect)
            return F.getBoundingClientRect();
        else {
            var A = {
                ROOT_TAG: /^body|html$/i,
                OP_SCROLL: /^(?:inline|table-row)$/i
            }, E = false, H = null, _ = F.offsetTop, G = F.offsetLeft, D = F.offsetWidth, B = F.offsetHeight, C = F.offsetParent;
            if (C != F)
                while (C) {
                    G += C.offsetLeft;
                    _ += C.offsetTop;
                    if (S(C, "position").toLowerCase() == "fixed")
                        E = true;
                    else if (C.tagName.toLowerCase() == "body")
                        H = C.ownerDocument.defaultView;
                    C = C.offsetParent
                }
            C = F.parentNode;
            while (C.tagName && !A.ROOT_TAG.test(C.tagName)) {
                if (C.scrollTop || C.scrollLeft)
                    if (!A.OP_SCROLL.test(Q(C)))
                        if (!b || C.style.overflow !== "visible") {
                            G -= C.scrollLeft;
                            _ -= C.scrollTop
                        }
                C = C.parentNode
            }
            if (!E) {
                var $ = a(H);
                G -= $.left;
                _ -= $.top
            }
            D += G;
            B += _;
            return {
                "left": G,
                "top": _,
                "right": D,
                "bottom": B
            }
        }
    }
    function N($) {
        $ = $ || V;
        var _ = $[O];
        _ = _[J] && _[J].clientHeight
                && _[J].clientHeight <= _.body.clientHeight ? _[J] : _.body;
        return {
            "width": _.clientWidth,
            "height": _.clientHeight
        }
    }
    function a($) {
        $ = $ || V;
        var B = $[O], A = B[J], _ = B.body;
        B = (A && A.scrollTop != null && (A.scrollTop > _.scrollTop || A.scrollLeft > _.scrollLeft))
                ? A
                : _;
        return {
            "top": B.scrollTop,
            "left": B.scrollLeft
        }
    }
    function D($) {
        src = $ ? ($.srcElement || $.target) : null;
        if ($dp && $dp.cal && !$dp.eCont && $dp.dd && Q($dp.dd) == "block"
                && src != $dp.el){
                    try
                    {
                       $dp.cal.close();
                     }
                    catch(ex){
                    }
            }
    }
    function Y() {
        $dp.status = 2;
        H()
    }
    function H() {
        if ($dp.flatCfgs.length > 0) {
            var $ = $dp.flatCfgs.shift();
            $.el = {
                innerHTML: ""
            };
            $.autoPickDate = true;
            $.qsEnabled = false;
            K($)
        }
    }
    var R, $;
    function U(G, A) {
        $dp.win = X;
        B();
        G = G || {};
        if (A) {
            if (!F()) {
                $ = $ || setInterval(function () {
                    if (V[O].readyState == "complete")
                        clearInterval($);
                    U(null, true)
                }, 50);
                return
            }
            if ($dp.status == 0) {
                $dp.status = 1;
                K({
                    el: {
                        innerHTML: ""
                    }
                }, true)
            } else
                return
        } else if (G.eCont) {
            G.eCont = $dp.$(G.eCont);
            $dp.flatCfgs.push(G);
            if ($dp.status == 2)
                H()
        } else {
            if ($dp.status == 0) {
                U(null, true);
                return
            }
            if ($dp.status != 2)
                return;
            var D = C();
            if (D) {
                $dp.srcEl = D.srcElement || D.target;
                D.cancelBubble = true
            }
            G.el = $dp.$(G.el || $dp.srcEl);
            if (!G.el
                    || G.el["My97Mark"] === true
                    || G.el.disabled
                    || (G.el == $dp.el && Q($dp.dd) != "none" && $dp.dd.style.left != "-1970px")) {
                $dp.el["My97Mark"] = false;
                return
            }
            K(G);
            if (G.el.nodeType == 1 && G.el["My97Mark"] === undefined) {
                $dp.el["My97Mark"] = false;
                try {
                    var _ = D.type == "focus" ? "onclick" : "onfocus";
                    E(G.el, _, function () {
                        U.call(this, G)
                    })
                } catch (e) { }
            }
        }
        function F() {
            if (T && V != X && V[O].readyState != "complete")
                return false;
            return true
        }
        function C() {
            if (I) {
                func = C.caller;
                while (func != null) {
                    var $ = func.arguments[0];
                    if ($ && ($ + "").indexOf("Event") >= 0)
                        return $;
                    func = func.caller
                }
                return null
            }
            return event
        }
    }
    function S(_, $) {
        return _.currentStyle ? _.currentStyle[$] : document.defaultView
                .getComputedStyle(_, false)[$]
    }
    function Q(_, $) {
        if (_)
            if ($ != null)
                _.style.display = $;
            else
                return S(_, "display")
    }
    function K(H, $) {
        for (var D in _)
            if (D.substring(0, 1) != "$")
                $dp[D] = _[D];
        for (D in H)
            if ($dp[D] !== undefined)
                $dp[D] = H[D];
        var E = $dp.el ? $dp.el.nodeName : "INPUT";
        if ($ || $dp.eCont
                || new RegExp(/input|textarea|div|span|p|a/ig).test(E))
            $dp.elProp = E == "INPUT" ? "value" : "innerHTML";
        else
            return;
        if ($dp.lang == "auto")
            $dp.lang = T
                    ? navigator.browserLanguage.toLowerCase()
                    : navigator.language.toLowerCase();
        if (!$dp.dd
                || $dp.eCont
                || ($dp.lang && $dp.realLang && $dp.realLang.name != $dp.lang
                        && $dp.getLangIndex && $dp.getLangIndex($dp.lang) >= 0)) {
            if ($dp.dd && !$dp.eCont)
                V[O].body.removeChild($dp.dd);
            if (_.$dpPath == "")
                F(A);
            var B = "<iframe style=\"width:1px;height:1px\" src=\""
                    + _.$dpPath
                    + "My97DatePicker.htm\" frameborder=\"0\" border=\"0\" scrolling=\"no\"></iframe>";
            if ($dp.eCont) {
                $dp.eCont.innerHTML = B;
                Z($dp.eCont.childNodes[0], Y)
            } else {
                $dp.dd = V[O].createElement("DIV");
                $dp.dd.style.cssText = "position:absolute;z-index:19700";
                $dp.dd.innerHTML = B;
                V[O].body.insertBefore($dp.dd, V[O].body.firstChild);
                Z($dp.dd.childNodes[0], Y);
                if ($)
                    $dp.dd.style.left = $dp.dd.style.top = "-1970px";
                else {
                    $dp.show();
                    C()
                }
            }
        } else if ($dp.cal) {
            $dp.show();
            $dp.cal.init();
            if (!$dp.eCont)
                C()
        }
        function C() {
            var F = $dp.position.left, B = $dp.position.top, C = $dp.el;
            if (C != $dp.srcEl && (Q(C) == "none" || C.type == "hidden"))
                C = $dp.srcEl;
            var H = W(C), $ = G(X), D = N(V), A = a(V), E = $dp.dd.offsetHeight, _ = $dp.dd.offsetWidth;
            if (isNaN(B)) {
                if (B == "above"
                        || (B != "under" && (($.topM + H.bottom + E > D.height) && ($.topM
                                + H.top - E > 0))))
                    B = A.top + $.topM + H.top - E - 3;
                else
                    B = A.top + $.topM + H.bottom;
                B += T ? -1 : 1
            } else
                B += A.top + $.topM;
            if (isNaN(F))
                F = A.left + Math.min($.leftM + H.left, D.width - _ - 5)
                        - (T ? 2 : 0);
            else
                F += A.left + $.leftM;
            $dp.dd.style.top = B + "px";
            $dp.dd.style.left = F + "px"
        }
    }
})()  