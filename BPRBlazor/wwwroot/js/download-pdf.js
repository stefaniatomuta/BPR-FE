const styleSheet = "css/pdf.css";

function exportHTMLtoPDF(html, fileName) {
    html = setStyleSheet(html)
    
    html2pdf()
        .from(html)
        .save(`${fileName}.pdf`);
}

function setStyleSheet(html) {
    const link = "<head><link rel=\"stylesheet\" href=\"" + styleSheet + "\"/></head>"
    console.log(link + html)
    return link + html
}