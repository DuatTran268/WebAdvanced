import { useMyQuery } from "../../../Utils/Utils";

const BadRequest = () => {
  let query = useMyQuery(),
  redirectTo = query.get('redirectTo')?? '/'
  return(
    <>

    </>
  )
}

export default BadRequest;