$(function(){
    $("pre code").each(function (i, block) {
        hljs.highlightBlock(block);
    });
    // Suppression des code vide
    $("code:empty").remove();
});

