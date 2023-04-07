import { Link } from "react-router-dom";

const TagList = ({tagList}) =>{
  // Array.isArray(tagList): isArray kiểm tra xem biến có phải là mảng ko, trả về true nếu biến là một mảng, trả false nếu biến ko là mảng
  if (tagList && Array.isArray(tagList) && tagList.length > 0)
    return (
      <>
        {tagList.map((item, index) => {
          return(
            <Link to= {`/tag/${item.urlSlug}`}
            title={item.name}
            className="btn btn-sm btn-outline-secondary me-1"
            key={index}>
              {item.name}
            </Link>
          )
        })}
      </>
    )

  else
    return (
      <></>
    );
  
}

export default TagList;


