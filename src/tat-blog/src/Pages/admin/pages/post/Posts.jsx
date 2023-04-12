import React, { useEffect } from "react";

const Posts = () => {
  useEffect(() =>{
    document.title ="Quản lý bài viết";
  }, []);
  

  return (
    <>
      <h1>
        Đây là khu vực quản lý bài viết
      </h1>
    </>
  );
}
export default Posts;