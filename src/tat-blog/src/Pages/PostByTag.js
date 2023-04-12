import React, {useEffect} from "react";
import { useParams } from "react-router-dom";
import PostSearch from '../Components/PostSearch'

function PostByTag() {
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
              Danh sách bài viết có tag {params.TagSlug}
              <span className="text-success">
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

export default PostByTag;
