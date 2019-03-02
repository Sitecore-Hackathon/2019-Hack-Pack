$sc(document).ready(function (event) {

    window.trackerItems = {
        $links: null,
        $popup: null
    };

    setInterval(function () {
        window.bindTracker();
    }, 250);

    window.bindTracker = function () {
        if (window.trackerItems.$links && window.trackerItems.$links.length) {
            window.trackerItems.$links.off('click');
        }

        window.trackerItems.$links = $sc("#RibbonPanel .scRibbonToolbar a");

        setTimeout(function () {
            window.trackerItems.$links.on("click", function () {
                var tracker = trackRibbonLink(this);
                bindTrackerInPopup(tracker.chunk, tracker.strip, tracker.header);
            });
        },
            0);
    };

    window.bindTrackerInPopup = function (chunk, strip, header) {
        if (window.trackerItems.$popup && window.trackerItems.$popup.length) {
            window.trackerItems.$popup.off('click');
        }

        window.trackerItems.$popup = $sc(".scPopup tr");

        setTimeout(function () {
            window.trackerItems.$popup.on("click", function () {
                $sc(this).attr("data-chunk", chunk);
                $sc(this).attr("data-strip", strip);
                $sc(this).attr("data-parent-header", header);

                trackRibbonTr(this);
            });
        },
            0);
    };

    window.trackRibbonLink = function (element) {
        var tracker = {};
        if ($sc(element).find(".header") !== undefined) {
            tracker.header = $sc(element).find(".header").text();
        }
        if ($sc(element).attr("onclick") !== undefined) {
            let quotedText = /'([^']*)'/;
            var matches = quotedText.exec($sc(element).attr("onclick"));
            tracker.click = matches[matches.length - 1];
        }
        if ($sc(element).parents(".chunk").find(".caption") !== undefined) {
            tracker.chunk = $sc(element).parents(".chunk").find(".caption").text();
        }
        if ($sc(element).find("img") !== undefined && $sc(element).find("img").attr("src") !== undefined) {
            tracker.icon = $sc(element).find("img").attr("src").replace("/temp/iconcache/", "");
        }
        if ($sc(element).parents(".scRibbonToolbarStrip") !== undefined) {
            tracker.strip = $sc(element).parents(".scRibbonToolbarStrip").attr("id").replace("Ribbon110D559FDEA542EA9C1C8A5DF7E70EF9_Strip_", "").replace("Strip", "");
        }
        tracker.tooltip = $sc(element).attr("title");

        console.log(tracker);
        postTrackingEvent(tracker);
        return tracker;
    };

    window.trackRibbonTr = function (element) {
        var tracker = {};
        console.log($sc(element));
        tracker.header = $sc(element).attr("data-parent-header");
        if ($sc(element).attr("onclick") !== undefined) {
            let quotedText = /'([^']*)'/;
            var matches = quotedText.exec($sc(element).attr("onclick"));
            tracker.click = matches[matches.length - 1];
        }
        tracker.chunk = $sc(element).attr("data-chunk");
        if ($sc(element).find(".scMenuItemIcon_Hover").find("img") !== undefined && $sc(element).find(".scMenuItemIcon_Hover").find("img").attr("src") !== undefined) {
            tracker.icon = $sc(element).find(".scMenuItemIcon_Hover").find("img").attr("src").replace("/temp/iconcache/", "");
        }
        tracker.strip = $sc(element).attr("data-strip");
        tracker.tooltip = $sc(element).attr("title");

        console.log(tracker);
        postTrackingEvent(tracker);
        return tracker;
    };

    window.postTrackingEvent = function (tracker) {
        $sc.ajax({
            type: "POST",
            url: "/services/eventlogger.ashx",
            data: JSON.stringify(tracker),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

            }
        });
    };

    bindTracker();

    $sc(".scRibbonNavigatorButtonsGroupButtons a").click(function (event) {
        bindTracker();
    });
});