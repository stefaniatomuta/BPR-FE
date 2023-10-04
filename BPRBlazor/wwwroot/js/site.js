function exportHTMLtoPDF(html, fileName) {
    html2pdf()
        .from(html)
        .save(`${fileName}.pdf`);
}

function getElementOffset(elementId) {
    const element = document.getElementById(elementId);
    return { left: element.offsetLeft, top: element.offsetTop }
}