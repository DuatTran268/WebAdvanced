import axios from "axios";

export async function getFeaturedPost(){
  try {
    //// call api
    const response = await axios.get(`https://localhost:7247/api/posts/featured/3`);

    const data = response.data;
    if(data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}

// tập tin widget.js chứa các hàm gọi api dành cho các widget ở sidebar


