import React, {useEffect} from "react";


const Comment = () => {
  useEffect(() =>{
    document.title ="Quản lý comment";
  }, []);

  return (
    <>
      <h1>
        Đây là khu vực comments
      </h1>
    </>
  );
}
export default Comment;