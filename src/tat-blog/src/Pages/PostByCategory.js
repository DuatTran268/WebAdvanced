import React, {useEffect, useState} from "react";
import { useParams } from "react-router-dom";

// import { useLocation } from "react-router-dom";
// import Pager from "../Components/Pager";
// import PostItem from '../Components/PostItem'

// import { getCategories } from "../Services/CategoriesWidgets";


const PostByCategory = () => {
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  const params = useParams();
  

  return (
    <h1>
      Bài viết thuộc category:  
      <span className="text-success">
        {params.slug}
      </span>
    </h1>
  )


  // const[cateList, setCateList] = useState([]);
  // const [metadata, setMetaData] = useState({});

  // function useQuery(){
  //   const {search} = useLocation();

  //   return React.useMemo(() => 
  //   new URLSearchParams(search), [search]);
  // }

  // let query = useQuery(),
  // urlSlug = query.get('urlSlug') ?? '',
  // p = query.get('p') ?? 1,
  // ps = query.get('ps') ?? 5;


  // useEffect(() => {
  //   document.title = 'Bài viết có category';

  //   getCategories(urlSlug, ps, p).then(data => {
  //     if(data){
  //       setCateList(data.items);
  //       setMetaData(data.metadata);
  //     }
  //     else
  //     setCateList([]);
  //   })
  // }, [urlSlug, p, ps]);

  // useEffect(() => {
  //   window.scrollTo(0, 0);
  // }, [cateList]);

  // if(cateList.length > 0)
  //   return (
  //     <div className="p-4">
  //     <h1>Bài viết của category</h1>
  //       {cateList.map((item, index) => {
  //         return (
  //           <PostItem postItem={item} key={index}/>
  //         );
  //       })}
  //       <Pager postquery={{'urlSlug': urlSlug}} metadata={metadata}/>
  //     </div>
  //   );
  // else return(
  //   <></>
  // );
 
}

export default PostByCategory;