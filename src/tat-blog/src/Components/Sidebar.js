import React from "react";
import CategoriesWidget from "./CategoriesWidget";
import SearchForm from "./SearchForm";
import FeaturedPostsWidget from "./FeaturedPostsWidget"

import RandomPostWidget from "./RandomPostWidget";

import TagCloudWidget from "./TagCloudWidget";

import TopAuthorWidget from "./TopAuthorWidget";

import Archives from "./Archives";

const Sidebar = () =>{
  return (
    <div className="pt-4 ps-2">
      
      <SearchForm/>
      
      <TagCloudWidget/>

      <CategoriesWidget/>

      <Archives/>

      <FeaturedPostsWidget/>
      
      <TopAuthorWidget/>
      
      <RandomPostWidget/>
      
    </div>
  )
}

export default Sidebar; // cho phép sidebar sử dụng trong tập tin khác