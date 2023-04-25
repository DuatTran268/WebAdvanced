/* eslint-disable no-undef */
import React, { useEffect, useState } from "react";
import { getPostFilter, deletePost, changePublished } from "../../../../Services/BlogRepository";
import Loading from "../../../../Components/Loading";
import { Button, Table } from "react-bootstrap";
import { Link, useParams } from "react-router-dom";
import PostFilerPane from "../../../../Components/admin/PostFilterPane";
import { useSelector } from "react-redux";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faGlobe,
  faLock,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import PagerAdmin from "../../../../Components/admin/PagerAdmin";

const Posts = () => {
  const [postsList, setPostList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true),
    postFilter = useSelector((state) => state.postFilter);

    // const [metadata, setMetadata] = useState({});
    // const [pageNumber, setPageNumber] = useState(1);
    // function updatePageNumber(inc) {
    //   setPageNumber((currentVal) => currentVal + inc);
    // }
  

  let { id } = useParams,
    p = 1,
    ps = 10;

  useEffect(() => {
    document.title = "Quản lý bài viết";

    getPostFilter(
      postFilter.keyword,
      postFilter.authorId,
      postFilter.categoryId,
      postFilter.year,
      postFilter.month,
      ps,
      p
    ).then((data) => {
      if (data) {
        setPostList(data.items);
        console.log("data: ", data);
      } else {
        setPostList([]);
      }
      setIsVisibleLoading(false);
    });
  }, [postFilter, p, ps]);

  // delete
  const handleDeletePost = (e, id) => {
    e.preventDefault();
    RemovePost(id);

    async function RemovePost(id) {
      if (window.confirm("Bạn có muốn xoá bài viết này ")) {
        const res = await deletePost(id);
        if (res) {
          alert("Đã xoá bài viết");
        } else {
          alert("Xảy ra lỗi khi xoá bài viết này");
        }
      }
    }
  };

  // change published 
  const handleChangePublish = (e, id) => {
    e.preventDefault();

    ChangePublished(id)

    async function ChangePublished(id){
      // await changePublished(id)
      const response = await changePublished(id);
        if (response) {
          alert("Bạn đã thay đổi trạng thái bài viết")
        }else{
          alert("Thay đổi trạng thái bài viết không thành công");
        }
    }
  }


  return (
    <>
      <h1>Đây là khu vực quản lý bài viết {id}</h1>
      <PostFilerPane />
      {isVisibleLoading ? (
        <Loading />
      ) : (
        <>
          <Table striped responsive bordered>
            <thead>
              <tr>
                <th>Tiêu đề</th>
                <th>Tác giả</th>
                <th>Chủ đề</th>
                <th>Xuất bản</th>
                <th>Xoá</th>
              </tr>
            </thead>
            <tbody>
              {postsList.length > 0 ? (
                postsList.map((item, index) => (
                  <tr key={index}>
                    <td>
                      <Link
                        to={`/admin/posts/edit/${item.id}`}
                        className="text-bold"
                      >
                        {item.title}
                      </Link>
                      <p className="text-muted">{item.shortDescription}</p>
                    </td>
                    <td>{item.author.fullName}</td>
                    <td>{item.category.name}</td>
                    <td>
                      <div className="text-center"
                      onClick={(e) => handleChangePublish(e, item.id)}
                      >
                        {item.published ? (
                          <div className="text-primary">
                            <FontAwesomeIcon icon={faGlobe} /> Public
                          </div>
                        ) : (
                          <div className="text-danger">
                            <FontAwesomeIcon icon={faLock} /> Private
                          </div>
                        )}
                      </div>
                    </td>
                    <td>
                      <div
                        className="text-center text-danger"
                        onClick={(e) => handleDeletePost(e, item.id)}
                      >
                        <FontAwesomeIcon icon={faTrash} />
                      </div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={4}>
                    <h4 className="text-danger text-center">
                      Không tìm thấy bài viết nào
                    </h4>
                  </td>
                </tr>
              )}
            </tbody>
          </Table>
          {/* <PagerAdmin
          metadata={posts.metadata}
          onPageChange={updatePageNumber}/> */}
        </>
      )}
    </>
  );
};
export default Posts;
