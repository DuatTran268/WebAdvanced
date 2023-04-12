import React, {useEffect, useState} from "react";
import PostItem from './PostItem';
import { getPosts } from "../../Services/BlogRepository";
import Pager from "../common/Pager"

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
        pageSize: 5,
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