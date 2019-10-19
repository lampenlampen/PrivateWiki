var strgPressed = false;

document.addEventListener('keydown', (e) => {
    if (e.key == "Control") {
        strgPressed = true;
    } else if (e.key == "e" && strgPressed) {
        alert("Strg+E pressed");
        window.external.notify("key:strg+e");
    } else if (e.key == "p" && strgPressed) {
        alert("Strg+P pressed");
        window.external.notify("key:strg+p");
    } else if (e.key == "s" && strgPressed) {
        alert("Strg+S pressed");
        window.external.notify("key:strg+s");
    }
});

document.addEventListener("keyup", (e) => {
    if (e.key == "Control") {
        strgPressed = false;
    }
});

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