FROM microsoft/dotnet

RUN mkdir -p /app
WORKDIR /app

COPY *.csproj /app
RUN dotnet restore --no-cache  

COPY . /app

RUN dotnet build

EXPOSE 5000/tcp
CMD dotnet run


# do we need to remove a container ot pickup image changes?
# exclude obj?
# run vs cmd ?
# todo look at available tags
# edit csprojs ?
# docker build -t artur/coresample .


# docker run -d --name core -p 5000:5000 artur/coresample