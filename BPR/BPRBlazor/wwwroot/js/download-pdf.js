function exportHTMLtoPDF(html) {
    html2pdf().from(html).save('Download.pdf');
}