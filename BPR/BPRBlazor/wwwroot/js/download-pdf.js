function exportHTMLtoPDF(html, fileName) {
    html2pdf()
        .from(html)
        .save(`${fileName}.pdf`);
}