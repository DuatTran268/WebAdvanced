import React, {useEffect} from "react";
import { useParams } from "react-router-dom";
import PostSearch from '../../Components/posts/PostSearch'
const PostByCategory = () => {
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  const params = useParams();
  console.log(">> check params",params)
  

  return (
    <div >
      <h1 className="mt-3">
        {
          params ? (
            <>
              Danh sách bài viết có categories 
              <span className="text-success">
              {params.id}
              </span>
            </>
          ) : (
            'Danh sách bài viết'
          )
        }
      </h1>
      <PostSearch params={params}/>
    </div>
  );
}

export default PostByCategory;