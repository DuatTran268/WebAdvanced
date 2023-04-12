import React from "react";
import CategoriesWidget from "../widgets/CategoriesWidget";
import SearchForm from "./SearchForm";
import FeaturedPostsWidget from "../widgets/FeaturedPostsWidget"

import RandomPostWidget from "../widgets/RandomPostWidget";

import TagCloudWidget from "../widgets/TagCloudWidget";

import TopAuthorWidget from "../widgets/TopAuthorWidget";

import Archives from "../widgets/Archives";

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