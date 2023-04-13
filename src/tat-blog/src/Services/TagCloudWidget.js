import { get_api } from "./Methods";

export async function getTagClould(){

  return get_api (`https://localhost:7247/api/tags`)

}

export async function getTagBySlug(urlSlug = '', pageSize = 5, pageNumber = 1){

  return get_api (`https://localhost:7247/api/tags/${urlSlug}?PageSize=${pageSize}&PageNumber=${pageNumber}`)

  
}

