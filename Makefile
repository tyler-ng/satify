# Project Variables
PROJECT_NAME ?= Satify
ORG_NAME ?= Satify
REPO_NAME ?= Satify

.PHONY: migrations db

migrations:
	cd ./Satify.Data && dotnet ef --startup-project ../Satify.Web/ migrations add ${mname} & cd ..
db:
	cd ./Satify.Data && dotnet ef --startup-project ../Satify.Web/ database update && cd ..
