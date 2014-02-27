/// <reference path="./jquery-2.0.3.js" />

function addCssLink(link) {
    var _cssLink = '<link rel=\"stylesheet\" href=\"' + link + '\" type=\"text/css\" />';
    $head = $('head');
    $link = $('link[href=' + link + ']', $head);
    if ($link.length == 0) {
        $head.append(_cssLink);
    }
}

function removeCssLink(link) {
    $('head link[href=' + link + ']').remove();
}