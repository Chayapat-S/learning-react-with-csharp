import './App.css';
import { pdfjs } from 'react-pdf';
import { useEffect, useState } from 'react';
import { Document, Page } from 'react-pdf';

function App() {

  pdfjs.GlobalWorkerOptions.workerSrc = `//unpkg.com/pdfjs-dist@${pdfjs.version}/build/pdf.worker.min.js`;
  async function checkPDFVersion(pdfUrl) {
    const loadingTask = pdfjs.getDocument(pdfUrl);

    try {
      const pdfDocument = await loadingTask.promise;

      // Get the PDF version from the document's info dictionary
      const pdfVersion = pdfDocument.pdfInfo.pdfVersion;

      console.log(`PDF Version: ${pdfVersion}`);
    } catch (error) {
      console.error('Error checking PDF version:', error);
    }
  }

  useEffect(() => {
    const filePath = 'Asset/test.pdf';

    fetch(filePath)
      .then(response => {
        if (response.status === 200) {
          console.log('The file exists.');
        } else {
          console.log('The file does not exist.');
        }
      })
      .catch(error => {
        console.error('Error checking file existence:', error);
      });


    // Usage
    const pdfUrl = 'path-to-your-pdf-file.pdf';
    checkPDFVersion(filePath);
  }, []);


  const [numPages, setNumPages] = useState();
  const [pageNumber, setPageNumber] = useState(1);

  function onDocumentLoadSuccess({ numPages }) {
    setNumPages(numPages);
  }
  return (
    <div>
      <Document file="Asset/MonthlyReportOutsource_20230802_chayapat-1.pdf" onLoadSuccess={onDocumentLoadSuccess}>
        <Page pageNumber={pageNumber} />
      </Document>
      <p>
        Page {pageNumber} of {numPages}
      </p>
    </div>
  );
}

export default App;



