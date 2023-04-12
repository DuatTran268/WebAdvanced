import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";
import Navbar from "./Components/Navbar";
import Sidebar from "./Components/Sidebar";
import Footer from "./Components/Footer";
import Index from "./Pages/Index";
import About from "./Pages/About";
import Contact from "./Pages/Contact"
import Blog from "./Pages/Blog";
import DetailPost from "./Pages/DetailPost";
import PostByCategory from "./Pages/PostByCategory";
import PostByAuthor from "./Pages/PostByAuthor";
import PostByTag from "./Pages/PostByTag";
import PostByTime from "./Pages/PostByTime";
import NotFound from "./Components/NotFound";
import Layout from "./Pages/Layout";
import Rss from "./Pages/Rss";

function App() {
  return (
    <div>
      <Router>
        <Navbar />

        <div className="container-fluid">
          <div className="row">
            <div className="col-9">
            <Routes>
              <Route path="/" element={<Layout/>}>
                <Route path="/" element={<Index/>}/>
                <Route path="/blog" element={<Index/>}/>
                <Route path="/blog/About" element={<About/>}/>
                <Route path="/blog/Contact" element={<Contact/>}/>
                <Route path="/blog/Rss" element={<Rss/>}/>
                <Route path="/posts" element ={<Blog/>}/>
                <Route path="/post/:slug" element={<DetailPost/>}/>
                <Route path="/author/:slug" element={<PostByAuthor/>}/>
                <Route path="/tag/:TagSlug" element={<PostByTag/>}/>
                <Route path="/category/:CategorySlug" element={<PostByCategory/>}/>
                <Route path="/archives/:year/:month" element={<PostByTime/>}/>
                <Route path="*" element={<NotFound/>}/>
              </Route>
            </Routes>
            </div>
            <div className="col-3 border-start">
              <Sidebar />
            </div>
          </div>
        </div>
        <Footer/>
      </Router>
    </div>
  );
}

export default App;

// test new branch