import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getPostBySlug } from "../Services/BlogRepository";
import { isEmptyOrSpaces } from "../Utils/Utils";

const DetailPost = () => {
  const params = useParams(); // get params tu url tren trinh duyet
  const [post, setPost] = useState(null);
  const { slug } = params;
  
  let imageUrl =
    !post || isEmptyOrSpaces(post.imageUrl)
      ? process.env.PUBLIC_URL + "/images/image1.png"
      : `https://localhost:7247/${post.imageUrl}`;

  // call api get post by slug
  useEffect(() => {
    getPostBySlug(slug).then((data) => {
      // console.log(">>> check data", data);
      window.scrollTo(0, 0);
      if (data) {
        setPost(data);
      } else setPost({});
    });
  }, [slug]);

  if (post) {
    return (
      <div className="mt-3">
        <h1 className="text-success">{post.title}</h1>
        <p className="card-text">
          <span className="row">
            <small className="text-danger col-4">
              Tác giả:
              <Link className="text-decoration-none">
                {post.author.fullName}
              </Link>
            </small>

            <small className="text-danger col-4">
              Đăng ngày:
              <span className="text-success">{post.postedDate}</span>
            </small>

            <small className="text-danger col-4">
              Lượt xem:
              <span className="text-success">{post.viewCount}</span>
            </small>
          </span>
        </p>

        <p className="card-text">
          <span>
            <small className="text-danger">Chủ đề:</small>
            <Link className="text-decoration-none">{post.category.name}</Link>
          </span>
        </p>

        <p className="card-text">
          <span>
            <small className="text-danger">Tags:</small>
            {post.tags.map((tag, index) => (
              <Link
                key={index}
                to={`/tag/${tag.urlSlug}`}
                className="text-decoration-none btn btn-outline-success me-2"
              >
                {tag.name}
              </Link>
            ))}
          </span>
        </p>

        <div className="text-danger mt-3 mb-3">
          Short Description
          <span className="text-dark">{post.meta}</span>
        </div>

        <img src={imageUrl} alt={post.title} width={"100%"} />

        <div className="text-danger mt-3">
          Description
          <span className="text-dark">{post.description}</span>
        </div>
      </div>
    );
  } else {
    return <></>;
  }
};

export default DetailPost;
