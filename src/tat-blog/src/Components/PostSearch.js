import React, {useEffect, useState} from "react";
import Pager from './Pager';
import PostItem from './PostItem';
import { getPosts } from "../Services/BlogRepository";
// const PostSearch = ({postQuery}) =>{
//   console.log("postQuery: ", postQuery);
  
//   const {
//     keyword, year, month, tagSlug, authorSlug, categorySlug
//   } = postQuery;

//   const [pageNumber, setPageNumber] = useState(1);
//   const [postList , setPostList] = useState({
//     item: [],
//     metadata: {}
//   });

//   useEffect (() => {

//     loadBlogPost();

//     async function loadBlogPost(){
//       const parameters = new URLSearchParams({
//         keyword: keyword || '',
//         authorSlug: authorSlug || '',
//         tagSlug: tagSlug || '',
//         categorySlug: categorySlug || '',
//         year: year || '',
//         month: month || '',
//         pageNumber: pageNumber || 1,
//         pageSize: 5,

//       });

//       const apiEndpoint = `https://localhost:7247/api/posts?${parameters}`;
//       const response = await fetch(apiEndpoint);
//       const data = await response.json();

//       setPostList(data.result);
//       window.To(0, 0);
//     }
//   }, [keyword, authorSlug, tagSlug, categorySlug, year, month, pageNumber]);


//   function updatePageNumber(increase){
//     setPageNumber((currentValue) => currentValue + increase);
//   }


//   return (
//     <div className="posts">
//     {postList.item}
//     <Pager
//       pageCount = {postList.metadata.pageCount}
//       hasNextPage = {postList.metadata.hasNextPage}
//       hasPrevPage = {postList.metadata.hasPrevPage}
//       onChangePage = {updatePageNumber}
//     />
//     </div>
//   );
// }


// export default PostSearch;


function PostSearch(props) {
  const { querySearch, params } = props;
  const [posts, setPosts] = useState({
    items: [],
    metadata: {},
  });
  const [pageNumber, setPageNumber] = useState(1);

  function updatePageNumber(inc) {
    setPageNumber((currentVal) => currentVal + inc);
  }

  useEffect(() => {
    loadPosts();

    async function loadPosts() {
      const query = new URLSearchParams({
        pageNumber: Object.fromEntries(querySearch || '').length > 0 ? 1 : pageNumber || 1,
        pageSize: 2,
        ...Object.fromEntries(querySearch || ''),
        ...params,
      });

      console.log(query.toString());
      getPosts(query).then((data) => {
        console.log(data)
        if (data) {
          setPosts(data);
        } else
          setPosts({
            items: [],
            metadata: {},
          });
      });
    }
  }, [pageNumber, params, querySearch]);

  if (posts.items.length > 0) {
    return (
      <div className='p-4'>
        {posts.items.map((item, index) => (
          <PostItem
            postItem={item}
            key={index}
          ></PostItem>
        ))}
        <Pager
          metadata={posts.metadata}
          onPageChange={updatePageNumber}
        />
      </div>
    );
  }
  return <></>;
}

export default PostSearch;