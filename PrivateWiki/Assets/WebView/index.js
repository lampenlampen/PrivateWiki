MathJax.Hub.Config({
    tex2jax: {
        inlineMath: [
            ['$', '$'],
            ["\(", "\)"]
        ],
        processEscapes: true
    }
});

function codeCopyClickFunction() {
    // TODO delete alert in prod
    alert("CodeBlock Copy-Button Clicked!");
    window.external.notify("codeButtonCopy");

}