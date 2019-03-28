MathJax.Hub.Config({
    tex2jax: {
        inlineMath: [
            ['$', '$'],
            ["\(", "\)"]
        ],
        processEscapes: true
    }
});

MathJax.Hub.Register.StartupHook("onLoad", function () {
    window.MathJax.Hub.Config({
        elements: document.querySelectorAll(".math")
    });
});

mermaid.initialize({
    startOnLoad: true
});

function codeCopyClickFunction() {
    // TODO delete alert in prod
    alert("CodeBlock Copy-Button Clicked!");
    window.external.notify("codeButtonCopy");

}