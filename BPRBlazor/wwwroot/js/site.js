function exportHTMLtoPDF(html, fileName) {
    html2pdf()
        .from(html)
        .save(`${fileName}.pdf`);
}

function getElementOffset(elementId) {
    const element = document.getElementById(elementId);
    return { x: element.offsetLeft, y: element.offsetTop, height: element.offsetHeight, width: element.offsetWidth }
}

function removeSelectedElement(elementId) {
    var element = document.getElementById(elementId);
    element.selectedIndex = 0;
}

function handleConfirmation(message) {
    return confirm(message)
}