import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Footer from "../Components/common/Footer";
import Index from "../Pages/Index";
import About from "../Pages/About";
import Contact from "../Pages/Contact";
import Blog from "../Pages/Blog";
import DetailPost from "../Pages/posts/DetailPost";
import PostByCategory from "../Pages/posts/PostByCategory";
import PostByAuthor from "../Pages/posts/PostByAuthor";
import PostByTag from "../Pages/posts/PostByTag";
import PostByTime from "../Pages/posts/PostByTime";
import NotFound from "../Components/common/NotFound";
import Layout from "../Pages/Layout";
import Rss from "../Pages/Rss";
import AdminLayout from "../Pages/admin/layout/Layout";
import AdminIndex from "../Pages/admin/pages/Index";
import Authors from "../Pages/admin/pages/Author";
import Categories from "../Pages/admin/pages/Categories";
import Comments from "../Pages/admin/pages/Comment";
import Tags from "../Pages/admin/pages/Tags";
import Posts from "../Pages/admin/pages/post/Posts"

const Routers = () => {
  return (
    <div>
      <Router>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route path="/" element={<Index />} />
            <Route path="/blog" element={<Index />} />
            <Route path="/blog/About" element={<About />} />
            <Route path="/blog/Contact" element={<Contact />} />
            <Route path="/blog/Rss" element={<Rss />} />
            <Route path="/posts" element={<Blog />} />
            <Route path="/post/:slug" element={<DetailPost />} />
            <Route path="/author/:slug" element={<PostByAuthor />} />
            <Route path="/tag/:TagSlug" element={<PostByTag />} />
            <Route
              path="/category/:CategorySlug"
              element={<PostByCategory />}
            />
            <Route path="/archives/:year/:month" element={<PostByTime />} />
            <Route path="*" element={<NotFound />} />
          </Route>

          {/* admin */}
          <Route path="/admin" element={<AdminLayout/>}>
            {/* <Route path="/admin" element={<AdminIndex.default/>}/> */}
            <Route path="/admin" element={<AdminIndex/>}/>
            <Route path="/admin/authors" element={<Authors/>}/>
            <Route path="/admin/categories" element={<Categories/>}/>
            <Route path="/admin/tags" element={<Tags/>}/>
            <Route path="/admin/posts" element={<Posts/>}/>
            <Route path="/admin/comments" element={<Comments/>}/>


          </Route>
        </Routes>
        <Footer />
      </Router>
    </div>
  );
}

export default Routers;