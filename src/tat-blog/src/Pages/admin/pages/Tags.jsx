import React, {useEffect} from "react";

const Tags = () => {
  useEffect(() =>{
    document.title ="Quản lý tags";
  }, []);

  return (
    <>
      <h1>
        Đây là khu vực quản lý các Tags
      </h1>
    </>
  );
}

export default Tags;