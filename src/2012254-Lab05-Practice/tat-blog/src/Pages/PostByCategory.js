import React, {useEffect, useState} from "react";
import { useLocation } from "react-router-dom";
import Pager from "../Components/Pager";
import PostItem from '../Components/PostItem'
import { getPosts } from "../Services/BlogRepository";


const PostByCategory = () => {
  const[postList, setPostList] = useState([]);
  const [metadata, setMetaData] = useState({});

  function useQuery(){
    const {search} = useLocation();

    return React.useMemo(() => 
    new URLSearchParams(search), [search]);
  }

  let query = useQuery(),
  urlSlug = query.get('urlSlug'),
  p = query.get('p') ?? 1,
  ps = query.get('ps') ?? 5;


  useEffect(() => {
    document.title = 'Bài viết có category';

    getPosts(urlSlug, ps, p).then(data => {
      if(data){
        setPostList(data.items);
        setMetaData(data.metadata);
      }
      else
        setPostList([]);
    })
  }, [urlSlug, p, ps]);

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [postList]);

  if(postList.length > 0)
    return (
      <div className="p-4">
      <h1>Bài viết của category</h1>
        {postList.map((item, index) => {
          return (
            <PostItem postItem={item} key={index}/>
          );
        })}
        <Pager postquery={{'urlSlug': urlSlug}} metadata={metadata}/>
      </div>
    );
  else return(
    <></>
  );
}

export default PostByCategory;