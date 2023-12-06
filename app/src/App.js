import './App.css';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout";
import Home from "./pages/Home";
// import Blogs from "./pages/Blogs";
// import Contact from "./pages/Contact";
// import NoPage from "./pages/NoPage";

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
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          {/* <Route path="blogs" element={<Blogs />} /> */}
          {/* <Route path="contact" element={<Contact />} /> */}
          {/* <Route path="*" element={<NoPage />} /> */}
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;



