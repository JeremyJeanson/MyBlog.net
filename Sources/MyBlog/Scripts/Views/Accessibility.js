$.accessibility = {
    SetDefaultFont : function () {
        $.accessibility.SetFont("/UserSettings/SetDefaultFont", function () {
            $("#AccessibilitySettingsFontUseDylexia").removeClass("btn btn-primary");
            $("#AccessibilitySettingsFontUseDylexia").addClass("btn btn-outline-primary");
            $("#AccessibilitySettingsFontUseDefault").removeClass("btn btn-outline-primary");            
            $("#AccessibilitySettingsFontUseDefault").addClass("btn btn-primary");
        });
    },
    SetDysFont : function () {
        $.accessibility.SetFont("/UserSettings/SetDyslexicFont", function () {
            $("#AccessibilitySettingsFontUseDefault").removeClass("btn btn-primary");
            $("#AccessibilitySettingsFontUseDefault").addClass("btn btn-outline-primary");
            $("#AccessibilitySettingsFontUseDylexia").removeClass("btn btn-outline-primary");
            $("#AccessibilitySettingsFontUseDylexia").addClass("btn btn-primary");
        });
    },
    SetFont : function (url, callback) {
        $.post(url, null, function (data) {
            $("#LayoutStylesheet").attr("href", data);
            callback();
        });
    }
};