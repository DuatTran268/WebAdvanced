import { get_api } from "./Methods";


export async function getTopAuthor(){
  return get_api (`https://localhost:7247/api/authors/best/4`)
  
}

export async function getPostByAuthorSlug (urlSlug = ''){

  return get_api (`https://localhost:7247/api/authors/${urlSlug}`)
}