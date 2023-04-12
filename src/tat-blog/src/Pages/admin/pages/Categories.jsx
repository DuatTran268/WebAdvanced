import React, {useEffect} from "react";


const Categories = () => {
  useEffect(() =>{
    document.title ="Quản lý chủ đế";
  }, []);  

  return (
    <>
      <h1>Đây là khu vực quản lý chủ đề</h1>
    </>
  );
}

export default Categories;