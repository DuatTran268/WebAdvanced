import React, {useEffect, useState} from "react";
import Pager from './Pager';


const PostSearch = ({postQuery}) =>{
  const {
    keyword, year, month, tagSlug, authorSlug, categorySlug
  } = postQuery;

  const [pageNumber, setPageNumber] = useState(1);
  const [postList , setPostList] = useState({
    item: [],
    metadata: {}
  });

  useEffect (() => {

    loadBlogPost();

    async function loadBlogPost(){
      const parameters = new URLSearchParams({
        keyword: keyword || '',
        authorSlug: authorSlug || '',
        tagSlug: tagSlug || '',
        categorySlug: categorySlug || '',
        year: year || '',
        month: month || '',
        pageNumber: pageNumber || 1,
        pageSize: 5,

      });

      const apiEndpoint = `https://localhost:7247/api/posts?${parameters}`;
      const response = await fetch(apiEndpoint);
      const data = await response.json();

      setPostList(data.result);
      window.scrollTo(0, 0);
    }
  }, [keyword, authorSlug, tagSlug, categorySlug, year, month, pageNumber]);


  function updatePageNumber(increase){
    setPageNumber((currentValue) => currentValue + increase);
  }


  return (
    <div className="posts">
    {postList.item}
    {/* <Pager
      pageCount = {postList.metadata.pageCount}
      hasNextPage = {postList.metadata.hasNextPage}
      hasPrevPage = {postList.metadata.hasPrevPage}
      onChangePage = {updatePageNumber}
    /> */}
    </div>
  );
}


export default PostSearch;