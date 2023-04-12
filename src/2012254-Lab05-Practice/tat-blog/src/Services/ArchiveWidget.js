import axios  from "axios";

export async function getArchives(){
  try {
    const response = await axios.get(`https://localhost:7247/api/posts/archives/12`);

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