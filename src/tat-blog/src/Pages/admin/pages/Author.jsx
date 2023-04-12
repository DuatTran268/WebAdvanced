import React, {useEffect} from "react";


const Author = () => {
  useEffect(() =>{
    document.title ="Quản lý tác giả";
  }, []);

  return (
    <>
      <h1>Khu vực quản lý tác giả</h1>
    </>
  );
};

export default Author;