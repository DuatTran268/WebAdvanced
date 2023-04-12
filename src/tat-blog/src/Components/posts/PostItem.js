import TagList from "../common/TagList";
import Card from "react-bootstrap/Card";
import { Link } from "react-router-dom";
import { isEmptyOrSpaces } from "../../Utils/Utils";

const PostList = ({ postItem }) => {
  console.log(postItem)
  let imageUrl = isEmptyOrSpaces(postItem.imageUrl)
    ? process.env.PUBLIC_URL + "/images/image1.png"
    : `https://localhost:7247/${postItem.imageUrl}`;

  // let postedDate = new Date(postItem.postedDate);

  const { urlSlug, title, shortDescription, category, author, tags } = postItem;

  return (
    <article className="blog-entry mb-4">
      <Card>
        <div className="row g-0">
          <div className="col-md-4">
            <Card.Img variant="top" src={imageUrl} alt={title} />
          </div>
          <div className="col-md-8">
            <Card.Body>
              <Card.Title>
                <Link
                  to={`/post/${urlSlug}`}
                  title="Xem chi tiết"
                  className="text-decoration-none"
                >
                  {title}
                </Link>
              </Card.Title>

              <Card.Text>
                <small className="text-muted">Tác giả: </small>
                <span className="text-primary m-1">
                  <Link
                    className="text-decoration-none"
                    to={`/author/${author.urlSlug}`}
                  >
                    {author.fullName}
                  </Link>
                </span>
                <span className="text-muted">Chủ đề: </span>
                <span className="text-primary m-1">
                  <Link
                    className="text-decoration-none"
                    to={`/category/${category.urlSlug}`}
                  >
                    {category.name}
                  </Link>
                </span>
              </Card.Text>

              <Card.Text>{shortDescription}</Card.Text>

              <div className="tag-list">
                <TagList tagList={tags} />
              </div>

              <div className="text-end">
                <Link
                  to={`/post/${urlSlug}`}
                  className="btn btn-primary"
                  title={title}
                >
                  Xem chi tiết
                </Link>
              </div>
            </Card.Body>
          </div>
        </div>
      </Card>
    </article>
  );
};

export default PostList;