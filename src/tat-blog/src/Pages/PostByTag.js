// import React, { useEffect, useState } from "react";
// import { useLocation, useParams } from "react-router-dom";
// import { getTagBySlug } from "../Services/TagCloudWidget";
// import PostItem from "../Components/PostItem";
// import Pager from "../Components/Pager";

// const PostByTag = () => {
//   const params = useParams();
//   const [tagList, setTagList] = useState([]);
//   const [metadata, setMetaData] = useState({});

//   function useQuery() {
//     const { search } = useLocation();

//     return React.useMemo(() => new URLSearchParams(search), [search]);
//   }

//   let query = useQuery(),
//     slug = query.get("slug") ?? "",
//     p = query.get("p") ?? 1,
//     ps = query.get("ps") ?? 5;

//   useEffect(() => {
//     document.title = "Bài viết thuộc tag";

//     getTagBySlug(slug, ps, p).then((data) => {
//       if (data) {
//         console.log("data: ", data);
//         setTagList(data);
//         setMetaData(data.metadata);
//       } else {
//         setTagList([]);
//       }
//     });
//   }, [slug, p, ps]);

//   useEffect(() => {
//     window.scrollTo(0, 0);
//   }, [tagList]);

//   if (tagList.length > 0) {
//     return (
//       <div className="p-4">
//         <h1>
//           Chi tiết của tag
//           <span className="text-primary">{params.slug}</span>
//         </h1>

//         {tagList.map((item, index) => {
//           return <PostItem postItem={item} key={index} />;
//         })}
//         <Pager postquery={{ urlSlug: slug }} metadata={metadata} />
//       </div>
//     );
//   } else return <></>;
// };

// export default PostByTag;

import React, {useEffect} from "react";
import { useParams } from "react-router-dom";
import PostSearch from '../Components/PostSearch'

function PostByTag() {
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  const params = useParams();
  // const location = useLocation();
  // const {tag} = location.state;
  console.log(params)

  return (
    // <h1>
    //   Bài viết thuộc tag
    //   <span className="text-success">
    //     {params.slug}
    //   </span>
    // </h1>
    <div >
      <h1 className="mt-3">
        {
          params ? (
            <>
              Danh sách bài viết có tag
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

export default PostByTag;
