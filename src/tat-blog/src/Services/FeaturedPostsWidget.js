import { get_api } from "./Methods";

export async function getFeaturedPost(){
  
  return get_api (`https://localhost:7247/api/posts/featured/3`)

}

// tập tin widget.js chứa các hàm gọi api dành cho các widget ở sidebar


