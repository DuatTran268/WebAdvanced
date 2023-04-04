import React from "react";
import CategoriesWidget from "./CategoriesWidget";
import SearchForm from "./SearchForm";


const Sidebar = () =>{
  return (
    <div className="pt-4 ps-2">
      
      <SearchForm/>
      
      <CategoriesWidget/>

      <h1>
        Bài viết nổi bật
      </h1>
      <h1>
        Đăng ký nhận tin mới
      </h1>
      <h1>
        Tag cloud
      </h1>
    </div>
  )
}

export default Sidebar; // cho phép sidebar sử dụng trong tập tin khác