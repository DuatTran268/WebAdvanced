import React, {useEffect} from "react";
import { useParams } from "react-router-dom";
import PostSearch from "../../Components/posts/PostSearch";

const PostByAuthor = () => {
  useEffect(() => {
    window.scroll(0, 0);
  },[]);
  
  const params = useParams();
  console.log(">> check params", params)

  return (
    <div>
      <h1 className="mt-3">
        {
          params ? (
            <>
              Danh sách bài viết của tác giả nhiều bài viết
              <span className="text-success">
                {params.id}
              </span>
            </>
          ) : (
            'Danh sách bài viết aaaa'
          )
        }
      </h1>
      <PostSearch params={params}/>
    </div>
  );
};


export default PostByAuthor;